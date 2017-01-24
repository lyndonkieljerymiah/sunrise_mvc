using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sunrise.VillaManagement.Data.Villas;
using Sunrise.VillaManagement.Persistence;

namespace Sunrise.VillaManagement.Data.Factory
{
    public class VillaDataFactory : IVillaDataFactory
    {
        private AppDbContext _app;
        private ReferenceDbContext _ref;
        private IVillaDataService _villaDataService;

        public VillaDataFactory()
        {
            _app = new AppDbContext();
            _ref = new ReferenceDbContext();
        }


        public IVillaDataService Villas
        {
            get
            {
                if (_villaDataService == null) _villaDataService = new VillaDataService(_app, _ref);
                return _villaDataService;
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
