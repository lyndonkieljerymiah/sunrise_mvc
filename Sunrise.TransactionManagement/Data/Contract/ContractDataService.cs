using PagedList;
using PagedList.EntityFramework;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Utilities.Enum;


namespace Sunrise.TransactionManagement.Data.Contract
{
    using DTO;
    using Model;
    using Persistence;
    using Infrastructure.Extension;
    using System.Linq.Expressions;
    using Compositor;
    using Enum;

    /// <summary>
    /// TODO: Contract Data Service abstract methods for Contract Db Operation
    /// Root: Contract
    /// </summary>
    public class ContractDataService : IContractDataService
    {
        private AppDbContext Context { get; set; }
        private ReferenceDbContext ReferenceContext { get; set; }

        public ContractDataService(AppDbContext context, ReferenceDbContext referenceContext)
        {
            Context = context;
            ReferenceContext = referenceContext;
        }
        //command
        public async Task<CustomResult> CreateContract(Contract transaction)
        {
            var result = new CustomResult();
            try
            {
                Context.Transactions.Add(transaction);
                await Context.SaveChangesAsync();
                result.ReturnObject = transaction.Id;
                result.Success = true;
            }
            catch (Exception e)
            {
                result.AddError("InternalErrorException", e.Message);
                result.Success = false;
            }
            return result;
        }
        public async Task<CustomResult> RemoveContract(string id)
        {
            var result = new CustomResult();
            try
            {
                var contract = await Context.Transactions.FindAsync(id);
                if (contract != null)
                {
                    Context.Transactions.Remove(contract);
                    await Context.SaveChangesAsync();
                    result.Success = true;
                    result.ReturnObject = contract;
                }
                else
                {
                    throw new Exception("Invalid Contract");
                }
            }
            catch (Exception e)
            {
                result.AddError("InternalErrorException", e.Message);
                result.Success = false;
            }
            return result;
        }
        public async Task<CustomResult> UpdateContract(Contract transaction)
        {
            var result = new CustomResult();
            try
            {
                Context.Transactions.Attach(transaction);
                Context.Entry(transaction).State = System.Data.Entity.EntityState.Modified;
                await Context.SaveChangesAsync();
                result.Success = true;
            }
            catch (Exception e)
            {
                result.AddError("InternalErrorException", e.Message);
                result.Success = false;
            }
            return result;
        }

        //query
        #region ver 1.2
        public ContractViewCollectionComposite GetViewContracts()
        {
            /*****************************************  
             * sql: select * from transactionview 
             *      inner join     
             ****************************************/
            var contracts = ReferenceContext
                            .Transactions
                            .Include(t => t.Tenant)
                            .Include(v => v.Villa)
                            .Include(v => v.Payments);

            var contractCollection = new ContractViewCollectionComposite(contracts);
            return contractCollection;
        }

        public async Task<TransactionView> FindContractViewByKey(string id)
        {
            var contract = await ReferenceContext
                           .Transactions
                           .Include(t => t.Tenant)
                           .Include(v => v.Villa)
                           .Include(v => v.Villa.Galleries)
                           .Include(v => v.Payments)
                           .SingleOrDefaultAsync(c => c.Id == id);

            return contract;
        }
        public async Task<Contract> FindContractByKey(string id, bool isPaymentIncluded = true)
        {
            if (isPaymentIncluded)
            {
                return await Context.Transactions
                    .Include(t => t.Payments)
                    .SingleOrDefaultAsync(t => t.Id == id);
            }
            else
            {
                return await Context.Transactions.FindAsync(id);
            }
        }
      

        public async Task<TransactionView> FindContractViewByCode(string code,ContractStatusEnum status = ContractStatusEnum.All)
        {
            var contract = ReferenceContext
                                .Transactions
                                .Include(t => t.Tenant)
                                .Include(v => v.Villa)
                                .Include(v => v.Payments);
            switch(status)
            {
                case ContractStatusEnum.Active:
                    return await contract.SingleOrDefaultAsync(c => c.StatusCode == ContractStatusSelection.Active && c.Code == code);
                default:
                    break;
            }

            return await contract.SingleOrDefaultAsync(c => c.Code == code);
        }
        #endregion



    }
}
