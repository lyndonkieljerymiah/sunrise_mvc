﻿using Sunrise.TransactionManagement.Abstract;
using Sunrise.TransactionManagement.Model;
using System;
using System.Threading.Tasks;
using Utilities.GeneralRepository;
using Sunrise.TransactionManagement.DTO;
using System.Data.Entity;
using System.Linq;

namespace Sunrise.TransactionManagement.Persistence.Repository
{
    public class TransactionRepository : BaseRepository<Transaction, AppDbContext>, ITransactionRepository
    {
        private ReferenceDbContext _referenceDbContext;

        public TransactionRepository(
            AppDbContext context,
            ReferenceDbContext referenceDbContext) : base(context)
        {
            this._referenceDbContext = referenceDbContext;
        }

        public async Task<Transaction> GetTransactionById(string id)
        {
            var transaction = await _set
                                    .Include(t => t.Payments)
                                    .SingleOrDefaultAsync(t => t.Id == id);

            return transaction;
        }

        public async Task<TransactionView> GetTransactionView(string id)
        {
            
            var transaction = await this._referenceDbContext
                                        .Transactions
                                        .Include(t => t.Tenant)
                                        .Include(v => v.Villa)
                                        .Include(p => p.Payments)
                                        .SingleOrDefaultAsync(t => t.Id == id);

            return transaction;

        }
    }
}