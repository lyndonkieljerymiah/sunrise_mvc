using PagedList;
using Sunrise.TransactionManagement.DTO;
using Sunrise.TransactionManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Enum;

namespace Sunrise.TransactionManagement.Data.Contract
{
    public interface IContractDataService
    {
        Task<CustomResult> CreateContract(Transaction transaction);
        Task<CustomResult> RemoveContract(string id);
        Task<CustomResult> UpdateContract(Transaction transaction);

        Task<Transaction> GetContractById(string id, bool isPaymentIncluded = true);

        Task<IPagedList<TransactionView>> GetContracts(string contractNo, int pageNumber, int pageSize);
        Task<TransactionView> GetContractViewById(string id);
        Task<TransactionView> GetActiveContract(string code);
        

        Task<IPagedList<TransactionListDTO>> GetExpiryContracts(int monthGracePeriod, int pageNumber, int pageSize);
        Task<IPagedList<TransactionListDTO>> GetContractsForListing(string contractNo, int pageNumber, int pageSize);
    }
}
