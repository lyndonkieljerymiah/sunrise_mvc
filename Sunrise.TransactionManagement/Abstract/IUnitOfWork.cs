using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunrise.TransactionManagement.Abstract
{
    public interface IUnitOfWork : IDisposable
    {
        ITransactionRepository Transactions { get; }
        IPaymentRepository Payments { get;}
        Task SaveChanges();
    }
}
