using Sunrise.TransactionManagement.Persistence.Repository.Abstract;
using Sunrise.TransactionManagement.Persistence.Repository.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunrise.TransactionManagement.Persistence.Repository
{
    public class UnitOfWork : IUnitOfWork
    {

        private AppDbContext Context { get; set; }

        public UnitOfWork()
        {
            Context = new AppDbContext();
        }

        public IBillRepository Bills
        {
            get
            {
                return new BillRepository(Context);
            }
        }
        public IContractRepository Contracts
        {
            get
            {
                return new ContractRepository(Context);
            }
        }

        public async Task Commit()
        {
            await Context.SaveChangesAsync();
        }

   
        #region disposed method
        private bool disposed = false;


        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    Context.Dispose();
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
