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

namespace Sunrise.Client.Persistence.Manager
{

    public class ContractDataManager
    {
        private readonly IUnitOfWork _uow;

        public ContractDataManager(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<CustomResult> AddContract(TransactionRegisterViewModel vmTransaction)
        {
            var result = new CustomResult();
            try
            {
                var transaction = Transaction.Map(vmTransaction.Villa.VillaNo,
                    vmTransaction.RentalType,
                    vmTransaction.ContractStatus,
                    vmTransaction.PeriodStart,
                    vmTransaction.PeriodEnd, vmTransaction.AmountPayable,
                    vmTransaction.Villa.Id, vmTransaction.Register.Id, vmTransaction.UserId);

                _uow.Transactions.Add(transaction);
                await _uow.SaveChanges();

                result.Success = true;
                result.ReturnObject = transaction.Id;
            }
            catch (Exception e)
            {
                result.AddError("InternalErrorException", e.Message);
            }

            return result;
        }
        public async Task<CustomResult> ValidatePayment(PaymentViewModel model)
        {
            var result = new CustomResult();

            try
            {
                var contract = await _uow.Transactions.GetContractById(model.TransactionId);
                var IsPaymentCovered = contract.IsPaymentDateCovered(model.CoveredPeriodFrom);

                if (IsPaymentCovered)
                {
                    result.AddError("PeriodException", "Period Date is already covered");
                    return result;
                }

                result.Success = true;
            }
            catch (Exception e)
            {
                result.AddError("", e.Message);
            }

            return result;
        }
        public async Task<CustomResult> AddPayment(BillingViewModel model, Action updateStatus)
        {
            var result = new CustomResult();
            try
            {
                var contract = await _uow.Transactions.GetContractById(model.Id);
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
                                payment.Amount, payment.Remarks);
                        if (payment.Status == "psc")
                            isTriggerUpdate = true;
                        if (!isOk) break;
                    }
                }

                if (isOk)
                {
                    await _uow.SaveChanges();
                }
                else
                {
                    result.AddError("PeriodException", "Period Date already exist");
                    return result;
                }

                if (isTriggerUpdate)
                    updateStatus(); //callback

                result.Success = true;
            }
            catch (Exception e)
            {
                result.AddError("InternalErrorException", e.Message);
            }
            return result;
        }

        public async Task<CustomResult> UpdateContractPaymentStatus(string transactionId, IEnumerable<PaymentViewModel> values)
        {
            var result = new CustomResult();
            try
            {
                var contract = await _uow.Transactions.GetContractById(transactionId);
                if (contract == null)
                    throw new Exception("Invalid Contract");

                foreach (var payment in values)
                {
                    contract.UpdatePaymentStatus(payment.Id, payment.StatusCode, payment.Remarks);
                }

                contract.ActivateStatus();
                await _uow.SaveChanges();
                result.Success = true;

            }
            catch (Exception e)
            {
                result.AddError("InternalErrorException", e.Message);
            }

            return result;
        }

        public async Task<IEnumerable<BillingViewModel>> GetContracts()
        {
            var transactions = await _uow.Transactions.GetContracts();
            var vm = Mapper.Map<IEnumerable<BillingViewModel>>(transactions);
            return vm;
        }
        public async Task<BillingViewModel> GetContractById(string transactionId)
        {
            try
            {
                var contract = await _uow.Transactions.GetTransactionView(transactionId);
                var viewModel = Mapper.Map<BillingViewModel>(contract);
                
                viewModel.QatarId = contract.Tenant.Individual.QatarId;
                viewModel.Birthday = contract.Tenant.Individual.Birthday;
                viewModel.CrNo = contract.Tenant.Company.CrNo;
                viewModel.ValidityDate = contract.Tenant.Company.ValidityDate;

                return viewModel;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public async Task<BillingViewModel> GetContractByCode(string villaNo)
        {
            try
            {
                var sales = await _uow.Transactions.GetContractByCode(villaNo);
                var vmSales = Mapper.Map<BillingViewModel>(sales);
                return vmSales;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

    }
}