using Sunrise.TransactionManagement.DTO;
using Sunrise.TransactionManagement.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities.GeneralRepository;

namespace Sunrise.TransactionManagement.Persistence.Repository.Abstract
{
    public interface IContractRepository : IBaseRepository<Contract>
    {
        Task<IEnumerable<ContractView>> GetContracts(string[] status = null);
        Task<IEnumerable<ContractView>> GetOfficialContracts();
        Task<IEnumerable<ContractView>> GetActiveContracts();
        Task<ContractSingleView> GetSingleContract(string id);

    }
}
