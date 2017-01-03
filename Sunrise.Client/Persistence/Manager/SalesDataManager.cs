using AutoMapper;
using Sunrise.Client.Domains.ViewModels;
using System;
using System.Threading.Tasks;
using Sunrise.TransactionManagement.Abstract;
using Sunrise.TransactionManagement.Model;
using Utilities.Enum;

namespace Sunrise.Client.Persistence.Manager
{


    public class SalesDataManager
    {
        private readonly IUnitOfWork _uow;

        public SalesDataManager(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<CustomResult> CreateAsync(SalesRegisterViewModel vmTransaction)
        {
            var result = new CustomResult();
            try
            {
                var transaction = Transaction.Map(
                    vmTransaction.RentalType,
                    vmTransaction.ContractStatus, vmTransaction.PeriodStart,
                    vmTransaction.PeriodEnd, vmTransaction.AmountPayable, vmTransaction.Villa.Id, vmTransaction.Register.Id,vmTransaction.UserId);

                _uow.Transactions.Add(transaction);
                await _uow.SaveChanges();

                result.Success = true;
                result.ReturnObject = transaction.Id;
            }
            catch (Exception e)
            {
                result.Errors.Add(e.Message);
            }

            return result;
        }

        public async Task<CustomResult> AddPaymentAsync(PaymentViewModel value)
        {
            var result = new CustomResult();
            try
            {
                var sales = await _uow.Transactions.GetTransactionById(value.SalesTransactionId);
                var success = sales.AddPayment(value.Term, value.PaymentMode, 
                        value.ChequeNo, value.Bank, 
                        value.CoveredPeriodFrom, 
                        value.CoveredPeriodTo, 
                        value.Amount, value.Remarks);

                if (success)
                    await _uow.SaveChanges();

                result.Success = success;
            }
            catch (Exception e)
            {
                result.Errors.Add(e.Message);
            }
            return result;
        }

        public async Task<SalesViewModel> GetSalesAsync(string transactionId)
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





        
    }
}