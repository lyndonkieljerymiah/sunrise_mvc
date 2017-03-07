using Sunrise.TransactionManagement.DTO;
using Sunrise.TransactionManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.GeneralRepository;

namespace Sunrise.TransactionManagement.Persistence.Repository.Abstract
{
    public interface IBillRepository : IBaseRepository<Bill>
    {
        Task<BillView> GetBill(string billCode);
        Task<BillView> GetBillByContractCode(string contractId);
    }
}
