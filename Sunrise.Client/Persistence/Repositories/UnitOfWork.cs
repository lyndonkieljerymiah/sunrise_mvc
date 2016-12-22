using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sunrise.Client.Persistence.Abstract;

namespace Sunrise.Client.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private AppDbContext _context;
        private ITenantRepository _tenantRepo;
        private SelectionRepository _selRepo;

        public UnitOfWork()
        {
            _context = new AppDbContext();    
        }
        public ITenantRepository Tenants
        {
            get { return _tenantRepo ?? (_tenantRepo = new TenantRepository(_context)); }
        }

        public ISelectionRepository Selections
        {
            get { return _selRepo ?? (_selRepo = new SelectionRepository(_context)); }
        }
        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        private bool disposed = false;
       

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
