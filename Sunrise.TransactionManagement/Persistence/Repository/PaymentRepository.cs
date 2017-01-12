using Sunrise.TransactionManagement.Abstract;
using Sunrise.TransactionManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.GeneralRepository;
using System.Data.Entity;

namespace Sunrise.TransactionManagement.Persistence.Repository
{
    public class PaymentRepository : BaseRepository<Payment, AppDbContext>, IPaymentRepository
    {
        public PaymentRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Payment> GetPaymentById(int id)
        {
            var payment = await _set
                .Include(p => p.Transaction)
                .SingleOrDefaultAsync(p => p.Id == id);
            return payment;

        }
    }
}
