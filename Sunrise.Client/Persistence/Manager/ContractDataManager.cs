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

namespace Sunrise.Client.Persistence.Manager
{

    public class ContractDataManager
    {   

        private IContractDataFactory Factory { get; }

        public ContractDataManager(IContractDataFactory factory)
        {
            Factory = factory;
        }

        public async Task<CustomResult> AddContract(TransactionRegisterViewModel vmTransaction, Func<string, Task> callback)
        {
            var transaction = Transaction.Map(vmTransaction.Villa.VillaNo,
                vmTransaction.RentalType,
                vmTransaction.ContractStatus,
                vmTransaction.PeriodStart,
                vmTransaction.PeriodEnd, vmTransaction.AmountPayable,
                vmTransaction.Villa.Id, vmTransaction.Register.Id, vmTransaction.UserId);
            var result = await Factory.Contracts.CreateContract(transaction);
            if (result.Success)
                if (callback != null) await callback(vmTransaction.Id);
            return result;
        }
        public async Task<CustomResult> RemoveContract(string id, Func<string,string,Task> callback = null)
        {
            var result = await Factory.Contracts.RemoveContract(id);
            if (result.Success)
            {
                var contract = (Transaction)result.ReturnObject;
                if (callback != null) await callback(contract.TenantId,contract.VillaId);
            }
            return result;
        }
        public async Task<CustomResult> AddPayment(BillingViewModel model, string userId, Func<string, Task> updateStatus)
        {
            var result = new CustomResult();
            try
            {
                var contract = await Factory.Contracts.GetContractById(model.Id,true);
                if (contract == null)
                    throw new Exception("Invalid Contract");

                //activate contract
                contract.ActivateStatus();

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

        #region contract
        public async Task<IEnumerable<ContractListViewModel>> GetContracts(string code,int pageNumber,int pageSize)
        {
            var transactions = await Factory.Contracts.GetContractsForListing(code, pageNumber, pageSize);
            return Mapper.Map<IEnumerable<ContractListViewModel>>(transactions);
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