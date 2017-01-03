using Sunrise.Maintenance.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunrise.Maintenance.Persistence.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _appDbContext;
        private ISelectionRepository _selections;
        public UnitOfWork(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public ISelectionRepository Selections
        {
            get
            {
                if(this._selections == null)
                {
                    this._selections = new SelectionRepository(_appDbContext);
                }
                return this._selections;
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
