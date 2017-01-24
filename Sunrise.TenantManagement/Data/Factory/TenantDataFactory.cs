using Sunrise.TenantManagement.Data.Tenants;
using Sunrise.TenantManagement.Persistence;
using System;

namespace Sunrise.VillaManagement.Data.Factory
{
    public class TenantDataFactory : ITenantDataFactory
    {
        private AppDbContext _app;
        private ITenantDataService _tenantDataService;

        public TenantDataFactory()
        {
            _app = new AppDbContext();
        }

        public ITenantDataService Tenants
        {
            get
            {
                if (_tenantDataService == null) _tenantDataService = new TenantDataService(_app);
                return _tenantDataService;
            }
        }

        #region disposed method
        private bool disposed = false;


        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _app.Dispose();
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
