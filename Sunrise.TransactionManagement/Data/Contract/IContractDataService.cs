using Sunrise.TransactionManagement.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities.Enum;

namespace Sunrise.TransactionManagement.Data.Contract
{
    public interface IContractDataService
    {
        //command
        Task<CustomResult> CreateContract(Model.Contract transaction);
        Task<CustomResult> RemoveContract(string id);
        Task<CustomResult> UpdateContract(Model.Contract transaction);

        //query ver 1.2
        Task<IEnumerable<ContractView>> GetViewContracts();
        Task<Model.Contract> FindContractByKey(string id);
        Task<ContractView> FindContractViewByKey(string id);
        Task<ContractView> FindContractViewByCode(string code);

       

    }
}
