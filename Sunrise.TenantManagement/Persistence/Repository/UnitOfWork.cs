using Sunrise.TenantManagement.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunrise.TenantManagement.Persistence.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppContextDb _appContextDb;
        private ITenantRepository _tenants;

        public UnitOfWork()
        {
            _appContextDb = new AppContextDb();
        }

        public ITenantRepository Tenants
        {
            get
            {
                if (this._tenants == null)
                {
                    this._tenants = new TenantRepository(_appContextDb);
                }
                return this._tenants;
            }
        }

    
        public async Task SaveChanges()
        {
            await _appContextDb.SaveChangesAsync();
        }

        public void SaveChangesNonAsync()
        {
            _appContextDb.SaveChanges();
        }
        #region disposed method
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _appContextDb.Dispose();
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
