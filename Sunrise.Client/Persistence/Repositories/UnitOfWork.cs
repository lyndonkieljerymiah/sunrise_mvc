using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sunrise.Client.Persistence.Abstract;
using Sunrise.Client.Persistence.Context;

namespace Sunrise.Client.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private AppDbContext _context;
        private ReferenceDbContext _referenceDbContext;
        private ITenantRepository _tenantRepo;
        private ISelectionRepository _selRepo;
        private IVillaRepository _villaRepo;
        private ITransactionRepository _transactionRepo;
        private IPaymentRepository _paymentRepo;
        

        public UnitOfWork()
        {
            _context = new AppDbContext();    
            _referenceDbContext = new ReferenceDbContext();
        }
        public ITenantRepository Tenants
        {
            get { return _tenantRepo ?? (_tenantRepo = new TenantRepository(_context, _referenceDbContext)); }
        }

        public ISelectionRepository Selections
        {
            get { return _selRepo ?? (_selRepo = new SelectionRepository(_context, _referenceDbContext)); }
        }

        public IVillaRepository Villas
        {
            get
            {
                return _villaRepo ?? (_villaRepo = new VillaRepository(_context, _referenceDbContext));
            }
        }

        public ITransactionRepository Transactions
        {
            get
            {
                return _transactionRepo ?? (_transactionRepo = new TransactionRepository(_context, _referenceDbContext));
            }
        }

        public IPaymentRepository Payments
        {
            get
            {
                return _paymentRepo ?? (_paymentRepo = new PaymentRepository(_context, _referenceDbContext));
            }
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
