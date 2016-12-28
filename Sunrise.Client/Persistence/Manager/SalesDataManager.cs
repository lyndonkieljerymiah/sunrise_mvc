using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using Sunrise.Client.Domains.Enum;
using Sunrise.Client.Domains.Models;
using Sunrise.Client.Domains.ViewModels;
using Sunrise.Client.Persistence.Abstract;

namespace Sunrise.Client.Persistence.Manager
{

  
    public class SalesDataManager
    {
        private readonly IUnitOfWork _uow;

        public SalesDataManager(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task CreateAsync(SalesTransaction transaction)
        {
            try
            {
                _uow.Transactions.Add(transaction);
                await _uow.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task UpdateAsync(SalesTransaction transaction)
        {
            try
            {

                _uow.Transactions.Update(transaction);
                await _uow.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<CustomResult> AddPaymentAsync(PaymentViewModel value)
        {
            var result = new CustomResult();
            try
            {   

                var payment = Mapper.Map<Payment>(value);
                _uow.Payments.Add(payment);
                await _uow.SaveChangesAsync();
                result.Success = true;
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
                var sales = await  _uow.Transactions.GetSalesById(transactionId);
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