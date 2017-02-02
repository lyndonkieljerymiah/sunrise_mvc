using Sunrise.TransactionManagement.Compositor;
using Sunrise.TransactionManagement.DTO;
using Sunrise.TransactionManagement.Enum;
using Sunrise.TransactionManagement.Model;
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

        //query
        ContractViewCollectionComposite GetViewContracts();
        Task<Model.Contract> FindContractByKey(string id, bool isPaymentIncluded = true);
        Task<TransactionView> FindContractViewByKey(string id);
        Task<TransactionView> FindContractViewByCode(string code, ContractStatusEnum status = ContractStatusEnum.All);

    }
}
