using Sunrise.TransactionManagement.DTO;
using Sunrise.TransactionManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.GeneralRepository;

namespace Sunrise.TransactionManagement.Abstract
{
    public interface ITransactionRepository : IBaseRepository<Transaction>
    {
        Task<Transaction> GetContractById(string id);
        Task<TransactionView> GetContractByCode(string villaNo);
        Task<TransactionView> GetTransactionView(string id);
        Task<IEnumerable<TransactionView>> GetContracts();

    }
}
