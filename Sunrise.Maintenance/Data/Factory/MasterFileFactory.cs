using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sunrise.Maintenance.Data.MasterFile;
using Sunrise.Maintenance.Persistence;

namespace Sunrise.Maintenance.Data.Factory
{
    public class MasterFileFactory : IMasterFileFactory
    {
        private AppDbContext _context;
        private ISelectionDataService _selectionDataService;

        public MasterFileFactory() {
            _context = new AppDbContext();
        }

        public ISelectionDataService Selections
        {
            get
            {
                if (_selectionDataService == null) _selectionDataService = new SelectionDataService(_context);
                return _selectionDataService;
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


        #endregion
    }
}
