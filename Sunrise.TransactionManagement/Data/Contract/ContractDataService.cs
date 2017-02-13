using System;
using System.Threading.Tasks;
using Utilities.Enum;


namespace Sunrise.TransactionManagement.Data.Contract
{
    using DTO;
    using Model;
    using Persistence;
    using System.Collections.Generic;

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
        public async Task<IEnumerable<ContractView>> GetViewContracts()
        {
            var contracts = await Context.Database.SqlQuery<ContractView>("dbo.GetAllContracts").ToListAsync();
            return contracts;
        }

        public async Task<ContractView> FindContractViewByKey(string id)
        {
            throw new NullReferenceException();
        }
        public async Task<Contract> FindContractByKey(string id)
        {
            throw new NullReferenceException();
        }
        public async Task<ContractView> FindContractViewByCode(string code)
        {
            throw new NullReferenceException();
        }
        
        #endregion


    }
}
