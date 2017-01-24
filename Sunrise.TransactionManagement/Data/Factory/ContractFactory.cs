using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sunrise.TransactionManagement.Data.Contract;
using Sunrise.TransactionManagement.Persistence;

namespace Sunrise.TransactionManagement.Data.Factory
{
    public class ContractDataFactory : IContractDataFactory
    {
        private AppDbContext _app;
        private ReferenceDbContext _ref;
        private IContractDataService _contractService;

        public ContractDataFactory()
        {
            _app = new AppDbContext();
            _ref = new ReferenceDbContext();
        }
        public IContractDataService Contracts
        {
            get
            {
                if (_contractService == null) _contractService = new ContractDataService(_app, _ref);
                return _contractService;
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
                    _ref.Dispose();
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
