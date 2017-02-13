using Sunrise.TransactionManagement.Model;
using Sunrise.TransactionManagement.Persistence.Repository.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.GeneralRepository;
using System.Data.Entity;
using Sunrise.TransactionManagement.DTO;
using System.Collections;
using System.Data.SqlClient;

namespace Sunrise.TransactionManagement.Persistence.Repository.Concrete
{   
    public class BillRepository : BaseRepository<Bill, AppDbContext>, IBillRepository
    {   
        public BillRepository(AppDbContext context) : base(context)
        {
            
        }

        public async Task<BillView> GetBill(string billId)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("BillCode",billId)
            };
            var bill = await _context.Database.SqlQuery<BillView>("GetBill @BillCode", parameters.ToArray()).SingleOrDefaultAsync();
            
            if(bill != null)
            {
                bill.Payments = (await GetPayments(bill.Id)).ToList();
                bill.Reconciles = (await GetReconciles(bill.Id)).ToList();
            }

            return bill;
        }
        public async Task<BillView> GetBillByContractCode(string contractId)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("ContractId",contractId)
            };
            var bill = await _context.Database.SqlQuery<BillView>("GetBillByContract @ContractId", parameters.ToArray()).SingleOrDefaultAsync();
            if (bill != null)
            {
                bill.Payments = (await GetPayments(bill.Id)).ToList();
                bill.Reconciles = (await GetReconciles(bill.Id)).ToList();
            }

            return bill;
        }
        public override void Update(Bill entity)
        {
            if (entity.Payments.Count > 0)
            {
                foreach (var payment in entity.Payments)
                {
                    if (payment.IsMarkDeleted)
                    {
                        entity.Payments.Remove(payment);
                        entity.Payments.Remove(payment);
                    }
                }
            }
            base.Update(entity);
        }
        
        #region private method
        private async Task<IEnumerable<PaymentView>> GetPayments(string billId)
        {
            var parameters = new List<SqlParameter> {
                new SqlParameter("BillId",billId) 
            };

            return await _context.Database.SqlQuery<PaymentView>("GetBillPayment @BillId", parameters.ToArray()).ToListAsync();

        }
        private async Task<IEnumerable<ReconcileView>> GetReconciles(string billId)
        {
            var parameters = new List<SqlParameter> {
                new SqlParameter("BillId",billId)
            };

            return await _context.Database.SqlQuery<ReconcileView>("GetBillReconcile @BillId", parameters.ToArray()).ToListAsync();
        }
        #endregion
    }
}
