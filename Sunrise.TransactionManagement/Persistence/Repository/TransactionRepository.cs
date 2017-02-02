using Sunrise.TransactionManagement.Abstract;
using Sunrise.TransactionManagement.Model;
using System;
using System.Threading.Tasks;
using Utilities.GeneralRepository;
using Sunrise.TransactionManagement.DTO;
using System.Data.Entity;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Sunrise.TransactionManagement.Persistence.Repository
{
    public class TransactionRepository : BaseRepository<Contract, AppDbContext>, ITransactionRepository
    {
        private ReferenceDbContext _referenceDbContext;

        public TransactionRepository(
            AppDbContext context,
            ReferenceDbContext referenceDbContext) : base(context)
        {
            this._referenceDbContext = referenceDbContext;
        }

        public async Task<TransactionView> GetContractByCode(string villaNo)
        {
            var transaction = await _referenceDbContext
                    .Transactions
                    .Include(t => t.Tenant)
                    .Include(t => t.Villa)
                    .Include(t => t.Payments)
                    .SingleOrDefaultAsync(t => t.Code == villaNo);

            return transaction;
        }

        public async Task<Contract> GetContractById(string id)
        {
            var transaction = await _set
                                    .Include(t => t.Payments)
                                    .SingleOrDefaultAsync(t => t.Id == id);

            return transaction;
        }
        
        public async Task<IEnumerable<TransactionView>> GetContracts(Expression<Func<TransactionView,bool>> clause=null)
        {

            var contracts = this._referenceDbContext
                                .Transactions
                                .Include(t => t.Tenant)
                                .Include(v => v.Villa)
                                .Include(v => v.Payments);

            if (clause != null)
            {
                contracts = contracts.Where(clause);
            }

            return await contracts.ToListAsync();
        }

        public async Task<TransactionView> GetTransactionView(Expression<Func<TransactionView,bool>> clause)
        {
            var transaction = await this._referenceDbContext
                                        .Transactions
                                        .Include(t => t.Tenant)
                                        .Include(v => v.Villa)
                                        .Include(p => p.Payments)
                                        .SingleOrDefaultAsync(clause);
            return transaction;
        }




    }
}
