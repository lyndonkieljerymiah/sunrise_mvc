using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
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
                throw new Exception("Error");
            }
        }

        public async Task<TransactionViewModel> GetSalesAsync(string transactionId)
        {
            try
            {
                var sales = await  _uow.Transactions.GetSalesById(transactionId);

                var vmSales = Mapper.Map<SalesViewModel>(sales);
                var vmTenant = Mapper.Map<TenantRegisterViewModel>(sales.Tenant);

                return new TransactionViewModel
                {
                    Sales = vmSales,
                    Tenant = vmTenant
                };

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }





        
    }
}