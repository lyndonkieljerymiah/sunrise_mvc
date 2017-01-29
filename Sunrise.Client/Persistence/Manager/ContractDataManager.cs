using AutoMapper;
using Sunrise.Client.Domains.ViewModels;
using System;
using System.Threading.Tasks;
using Sunrise.TransactionManagement.Abstract;
using Sunrise.TransactionManagement.Model;
using Utilities.Enum;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sunrise.TransactionManagement.Data.Factory;
using Sunrise.TransactionManagement.DTO;
using PagedList;
using Sunrise.Client.Infrastructure.Extension;
using Sunrise.Client.Domains.Configuration;
using Utilities.Helper;

namespace Sunrise.Client.Persistence.Manager
{

    public class ContractDataManager
    {

        private IContractDataFactory Factory { get; }
        private ContractConfig Config { get; }
        public ContractDataManager(IContractDataFactory factory)
        {
            Factory = factory;

            Config = new ContractConfig();
            Config.DefaultMonth = 12;

        }

        #region contract
        public TransactionRegisterViewModel CreateNewContract(string villaNo,DateTime periodStart, decimal ratePerMonth)
        {
            var newContract = Transaction.CreateNew(villaNo,Config.DefaultMonth, ratePerMonth, periodStart, new MonthRateCalculation());
            var viewModel = Mapper.Map<TransactionRegisterViewModel>(newContract);

            return viewModel;
        }

        public async Task<CustomResult> AddContract(TransactionRegisterViewModel vmTransaction,Func<string, Task> callback)
        {
            //create code
            var code = "C" + vmTransaction.Villa.VillaNo + DateTime.Today.Year;
            var transaction = Transaction.Map(code,
                vmTransaction.RentalType,
                vmTransaction.ContractStatus,
                vmTransaction.PeriodStart,
                vmTransaction.PeriodEnd, vmTransaction.AmountPayable,
                vmTransaction.Villa.Id, vmTransaction.Register.Id, vmTransaction.UserId);

            var result = await Factory.Contracts.CreateContract(transaction);
            if (result.Success)
                if (callback != null) await callback(vmTransaction.Villa.Id);

            return result;
        }
        public async Task<CustomResult> RemoveContract(string id, Func<string, string, Task> callback = null)
        {
            var result = await Factory.Contracts.RemoveContract(id);
            if (result.Success)
            {
                var contract = (Transaction)result.ReturnObject;
                if (callback != null) await callback(contract.TenantId, contract.VillaId);
            }
            return result;
        }
        public async Task<CustomResult> AddPayment(BillingViewModel model, string userId, Func<string, Task> updateStatus)
        {
            var result = new CustomResult();
            try
            {
                var contract = await Factory.Contracts.GetContractById(model.Id, true);
                if (contract == null)
                    throw new Exception("Invalid Contract");

                //activate contract
                contract.ActivateStatus();
                contract.ReversedContract();
                bool isOk = false;
                bool isTriggerUpdate = false;

                foreach (var payment in model.Payments)
                {
                    //insert only new 
                    if (payment.Id == 0)
                    {
                        isOk = contract.AddPayment(payment.PaymentDate, payment.PaymentTypeCode, payment.PaymentModeCode,
                                payment.ChequeNo, payment.BankCode,
                                payment.CoveredPeriodFrom,
                                payment.CoveredPeriodTo,
                                payment.Amount, payment.Remarks, userId);

                        if (payment.Status == "psc")
                            isTriggerUpdate = true;
                        if (!isOk) break;
                    }
                    else
                    {
                        isOk = contract.UpdatePayment(payment.Id, payment.PaymentDate, payment.PaymentTypeCode, payment.PaymentModeCode,
                                payment.ChequeNo, payment.BankCode,
                                payment.CoveredPeriodFrom,
                                payment.CoveredPeriodTo,
                                payment.Amount, payment.Remarks, userId);
                        if (payment.Status == "psc")
                            isTriggerUpdate = true;
                        if (!isOk) break;
                    }
                }

                if (isOk)
                {
                    result = await Factory.Contracts.UpdateContract(contract);
                }
                else
                {
                    result.AddError("PeriodException", "Period Date already exist");
                    return result;
                }

                if (isTriggerUpdate)
                    await updateStatus(contract.VillaId); //callback

                result.Success = true;
            }
            catch (Exception e)
            {
                result.AddError("InternalErrorException", e.Message);
            }
            return result;
        }
        public async Task<CustomResult> UpdateContractPaymentStatus(string transactionId, string userId, IEnumerable<PaymentViewModel> values)
        {
            var result = new CustomResult();
            try
            {
                var contract = await Factory.Contracts.GetContractById(transactionId);
                if (contract == null)
                    throw new Exception("Invalid Contract");

                foreach (var payment in values)
                {
                    contract.UpdatePaymentStatus(payment.Id, payment.StatusCode, payment.Remarks, userId);
                }

                contract.ActivateStatus();
                await Factory.Contracts.UpdateContract(contract);
                result.Success = true;
            }
            catch (Exception e)
            {
                result.AddError("InternalErrorException", e.Message);
            }

            return result;
        }
        public async Task<IPagedList<ContractListViewModel>> GetContracts(string code, int pageNumber, int pageSize)
        {
            var transactions = await Factory.Contracts.GetContractsForListing(code, pageNumber, pageSize);
            return transactions.ToMappedPagedList<TransactionListDTO, ContractListViewModel>();
        }
        
        #region renewal
        public async Task<IPagedList<ContractListViewModel>> GetContractExpiry(int pageNumber, int pageSize)
        {
            var contracts = await Factory.Contracts.GetExpiryContracts(12, pageNumber, pageSize);
            return contracts.ToMappedPagedList<TransactionListDTO, ContractListViewModel>();
        }
        public async Task<TransactionRegisterViewModel> GetContractForRenewal(string contractId)
        {
            try
            {
                //take the old contract
                var oldContract = await Factory.Contracts.GetContractViewById(contractId);
                
                //copy to new
                var newContract = Transaction.CreateRenew(oldContract.Id, oldContract.Code,Config.DefaultMonth, oldContract.Villa.RatePerMonth, oldContract.PeriodEnd, new MonthRateCalculation());
                var vmNewContract = Mapper.Map<TransactionRegisterViewModel>(newContract);
                
                
                //take the tenant and villa
                vmNewContract.Register = Mapper.Map<TenantRegisterViewModel>(oldContract.Tenant);
                vmNewContract.TenantId = vmNewContract.Register.Id;
                vmNewContract.Villa = Mapper.Map<VillaViewModel>(oldContract.Villa);
                vmNewContract.VillaId = vmNewContract.Villa.Id;
                vmNewContract.RentalType = oldContract.RentalType;
                vmNewContract.RentalTypeCode = oldContract.RentalTypeCode;
                vmNewContract.ContractStatus = oldContract.ContractStatus;
                vmNewContract.ContractStatusCode = oldContract.ContractStatusCode;
                vmNewContract.Status = oldContract.Status;

                return vmNewContract;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }
        public async Task<CustomResult> RenewContract(TransactionRegisterViewModel register,string userId, Func<string, Task> callback = null)
        {
            var result = new CustomResult();
            
            //take the old one and update
            var oldContract = await Factory.Contracts.GetContractById(register.Id);
            var isSuccess = oldContract.ContractCompletion();
            
            //make sure the contract is completed before creating new one
            if (isSuccess)
            {
                
                //create new one
                var transaction = Transaction.Map(
                    register.Code, register.RentalTypeCode,
                    register.ContractStatusCode, register.PeriodStart, register.PeriodEnd,
                    register.AmountPayable,
                    register.VillaId, register.TenantId, userId);

                var tuple = Tuple.Create(oldContract, transaction);
                
                result = await Factory.Contracts.CreateContract(transaction);
                
            }
            else
            {
                result.Success = false;
                result.AddError("renewContractException", "Payment must be clear to renew.");
            }

            return result;
        }
        
        #endregion
        public async Task<CustomResult> TerminateContract(string contractId, string description, string userId, Func<string, Task> callback)
        {
            var result = new CustomResult();
            var contract = await Factory.Contracts.GetContractById(contractId);
            contract.TerminateContract(description, userId);
            result = await Factory.Contracts.UpdateContract(contract);
            return result;
        }
        #endregion

        #region billing module
        public async Task<BillingViewModel> GetContractForBilling(string contractId)
        {
            var contract = await Factory.Contracts.GetContractViewById(contractId);
            var viewModel = Mapper.Map<BillingViewModel>(contract);

            viewModel.QatarId = contract.Tenant.Individual.QatarId;
            viewModel.Birthday = contract.Tenant.Individual.Birthday;
            viewModel.CrNo = contract.Tenant.Company.CrNo;
            viewModel.ValidityDate = contract.Tenant.Company.ValidityDate;

            return viewModel;

        }
        #endregion

        #region receivable module
        public async Task<CustomResult> ReverseContract(string id, Func<string, Task> callback = null)
        {
            var contract = await Factory.Contracts.GetContractById(id);
            contract.ReversedContract();
            var result = await Factory.Contracts.UpdateContract(contract);
            if (result.Success)
            {
                if (callback != null) await callback(contract.VillaId);
            }
            return result;
        }
        public async Task<BillingViewModel> GetContractForPaymentClearing(string contractCode)
        {
            var contract = await Factory.Contracts.GetActiveContract(contractCode);
            var viewModel = Mapper.Map<BillingViewModel>(contract);

            viewModel.QatarId = contract.Tenant.Individual.QatarId;
            viewModel.Birthday = contract.Tenant.Individual.Birthday;
            viewModel.CrNo = contract.Tenant.Company.CrNo;
            viewModel.ValidityDate = contract.Tenant.Company.ValidityDate;

            return viewModel;

        }
        #endregion
    }
}