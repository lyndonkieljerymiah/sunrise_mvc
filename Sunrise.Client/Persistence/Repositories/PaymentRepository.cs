using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sunrise.Client.Domains.Models;
using Sunrise.Client.Persistence.Abstract;
using Sunrise.Client.Persistence.Context;

namespace Sunrise.Client.Persistence.Repositories
{
    public class PaymentRepository : BaseRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(AppDbContext context, ReferenceDbContext referenceDb) : base(context, referenceDb)
        {
        }
    }
}
