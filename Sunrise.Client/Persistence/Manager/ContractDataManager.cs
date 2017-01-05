using AutoMapper;
using Sunrise.Client.Domains.ViewModels;
using System;
using System.Threading.Tasks;
using Sunrise.TransactionManagement.Abstract;
using Sunrise.TransactionManagement.Model;
using Utilities.Enum;
using System.Collections;
using System.Collections.Generic;

namespace Sunrise.Client.Persistence.Manager
{

    public class ContractDataManager
    {
        private readonly IUnitOfWork _uow;

        public ContractDataManager(IUnitOfWork uow)
        {
            _uow = uow;
        }
        public async Task<CustomResult> AddContract(SalesRegisterViewModel vmTransaction)
        {
            var result = new CustomResult();
            try
            {
                var transaction = Transaction.Map(vmTransaction.Villa.VillaNo,
                    vmTransaction.RentalType,
                    vmTransaction.ContractStatus, 
                    vmTransaction.PeriodStart,
                    vmTransaction.PeriodEnd, vmTransaction.AmountPayable, 
                    vmTransaction.Villa.Id, vmTransaction.Register.Id,vmTransaction.UserId);

                _uow.Transactions.Add(transaction);
                await _uow.SaveChanges();

                result.Success = true;
                result.ReturnObject = transaction.Id;
            }
            catch (Exception e)
            {
                result.AddError(e.Message);
            }

            return result;
        }
        
        public async Task<CustomResult> AddPayment(PaymentViewModel value)
        {
            var result = new CustomResult();
            try
            {
                var sales = await _uow.Transactions.GetContractById(value.SalesTransactionId);
                var success = sales.AddPayment(value.Term, value.PaymentMode, 
                        value.ChequeNo, value.Bank, 
                        value.CoveredPeriodFrom, 
                        value.CoveredPeriodTo, 
                        value.Amount, value.Remarks);

                if (success)
                    await _uow.SaveChanges();
                else
                    result.AddError("Period Date already exist");
                result.Success = success;
            }
            catch (Exception e)
            {
                result.AddError(e.Message);
            }
            return result;
        }
        public async Task<CustomResult> UpdatePayments(string transactionId, IEnumerable<PaymentViewModel> values)
        {
            var result = new CustomResult();
            try
            {
                var contract = await _uow.Transactions.GetContractById(transactionId);
                if(contract == null)
                    throw new Exception("Invalid Contract");

                foreach (var payment in values)
                {
                    contract.UpdatePayment(payment.Id, payment.StatusCode,payment.Remarks);
                }
                contract.ActivateStatus();
                await _uow.SaveChanges();

                result.Success = true;
            }
            catch (Exception e)
            {
                result.AddError(e.Message);
            }

            return result;
        }

        public async Task<CustomResult> ActivateContract(string id)
        {
            var contract = await _uow.Transactions.FindQueryAsync(id);
            var result = new CustomResult();
            if (contract == null)
            {
                result.Success = false;
                result.AddError("Contract not found");
            }
            else
            {
                contract.ActivateStatus();
                await _uow.SaveChanges();
                result.Success = true;
            }
            return result;
        }
        public async Task<SalesViewModel> GetContractById(string transactionId)
        {
            try
            {
                var sales = await  _uow.Transactions.GetTransactionView(transactionId);
                var vmSales = Mapper.Map<SalesViewModel>(sales);
                return vmSales;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public async Task<SalesViewModel> GetContractByCode(string villaNo)
        {
            try
            {
                var sales = await _uow.Transactions.GetContractByCode(villaNo);
                var vmSales = Mapper.Map<SalesViewModel>(sales);
                return vmSales;
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        
    }
}