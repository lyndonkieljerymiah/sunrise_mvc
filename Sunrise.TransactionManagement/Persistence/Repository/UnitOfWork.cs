using Sunrise.TransactionManagement.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunrise.TransactionManagement.Persistence.Repository
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly AppDbContext _appDbContext;
        private readonly ReferenceDbContext _referenceContext;
        private ITransactionRepository _transactions;

        public UnitOfWork()
        {
            this._appDbContext = new AppDbContext();
            this._referenceContext = new ReferenceDbContext();
        }

        public ITransactionRepository Transactions
        {
            get
            {
                if(this._transactions == null)
                {
                    _transactions = new TransactionRepository(_appDbContext, _referenceContext);
                }
                return this._transactions;
            }
        }

        public async Task SaveChanges()
        {
            await _appDbContext.SaveChangesAsync();
        }

        #region disposed method
        private bool disposed = false;
      

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _appDbContext.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        #endregion
    }
}
