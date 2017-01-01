using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.EnterpriseServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sunrise.Client.Domains.DTO;
using Sunrise.Client.Domains.Models;
using Sunrise.Client.Persistence.Abstract;
using Sunrise.Client.Persistence.Context;

namespace Sunrise.Client.Persistence.Repositories
{
    public class TransactionRepository : BaseRepository<SalesTransaction>, ITransactionRepository
    {
        public TransactionRepository(AppDbContext context, ReferenceDbContext referenceDb) : base(context, referenceDb)
        {

            _referenceDbContext.Configuration.ProxyCreationEnabled = false;
        }

        /// <summary>
        /// TODO: Get sales transaction by its Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<SalesTransactionDTO> GetSalesById(string id)
        {
            
            var sales = await _referenceDbContext.SalesTransactionDtos
                .Include(s => s.Villa)
                .Include(s => s.Tenant)
                .Include(s => s.Tenant.Individual)
                .Include(s => s.Tenant.Company)
                .Include(s => s.Payments)
                .SingleOrDefaultAsync(s => s.Id == id);

            return sales;
        }
    }
}
