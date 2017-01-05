using Sunrise.TransactionManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.GeneralRepository;

namespace Sunrise.TransactionManagement.Abstract
{
    public interface IPaymentRepository : IBaseRepository<Payment>
    {
        Task<Payment> GetPaymentById(int id); 
    }
}
