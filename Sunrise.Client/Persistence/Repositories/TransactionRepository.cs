using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.EnterpriseServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sunrise.Client.Domains.Models;
using Sunrise.Client.Persistence.Abstract;

namespace Sunrise.Client.Persistence.Repositories
{
    public class TransactionRepository : BaseRepository<SalesTransaction>, ITransactionRepository
    {
        public TransactionRepository(AppDbContext context) : base(context)
        {
        }

        /// <summary>
        /// TODO Get sales transaction by its Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<SalesTransaction> GetSalesById(string id)
        {
            var sales = await _context.Sales
                .Include(s => s.Tenant)
                .Include(s => s.Villa)
                .SingleOrDefaultAsync(s => s.Id == id);

            return sales;
        }
    }
}
