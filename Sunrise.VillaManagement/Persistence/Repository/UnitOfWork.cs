using Sunrise.VillaManagement.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunrise.VillaManagement.Persistence.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private AppDbContext _appContextDb;
        private ReferenceDbContext _referenceDbContext;
        private IVillaRepository _villaRepo;
        

        public UnitOfWork()
        {
            _appContextDb = new AppDbContext();
            _referenceDbContext = new ReferenceDbContext();

        }

        public IVillaRepository Villas
        {
            get
            {
                if(this._villaRepo == null)
                {
                    this._villaRepo = new VillaRepository(_appContextDb,_referenceDbContext);
                }
                return this._villaRepo;
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
