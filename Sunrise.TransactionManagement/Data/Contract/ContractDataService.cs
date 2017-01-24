using Sunrise.TransactionManagement.Abstract;
using Sunrise.TransactionManagement.DTO;
using Sunrise.TransactionManagement.Model;
using Sunrise.TransactionManagement.Persistence;
using Sunrise.TransactionManagement.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using Utilities.Enum;
using PagedList.EntityFramework;

namespace Sunrise.TransactionManagement.Data.Contract
{

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

        public async Task<CustomResult> CreateContract(Transaction transaction)
        {
            var result = new CustomResult();
            try
            {
                Context.Transactions.Add(transaction);
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
        public async Task<CustomResult> UpdateContract(Transaction transaction)
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
        public async Task<IEnumerable<TransactionView>> GetContracts(string contractNo, int pageNumber, int pageSize)
        {
            var contracts = ReferenceContext
                            .Transactions
                            .Include(t => t.Tenant)
                            .Include(v => v.Villa)
                            .Include(v => v.Payments);

            if (!string.IsNullOrEmpty(contractNo))
            {
                contracts = contracts.Where(c => c.Code.Contains(contractNo));
            }

            return (await contracts
                .OrderBy(c => c.Code)
                .ToPagedListAsync(pageNumber, pageSize)).ToList();
        }
        public async Task<IEnumerable<TransactionListDTO>> GetContractsForListing(string contractNo, int pageNumber, int pageSize)
        {
            var contracts = ReferenceContext
                            .Transactions
                            .Include(t => t.Tenant)
                            .Include(v => v.Villa)
                            .Include(v => v.Payments)
                            .Select(t => new TransactionListDTO
                            {
                                Id = t.Id,
                                Code = t.Code,
                                PeriodStart = t.PeriodStart,
                                PeriodEnd = t.PeriodEnd,
                                DateCreated = t.DateCreated,
                                TenantName = t.Tenant.Name,
                                VillaNo = t.Villa.VillaNo,
                                Status = t.Status,
                                StatusCode = t.StatusCode,
                                AmountPayable = t.AmountPayable
                            });
            if (!string.IsNullOrEmpty(contractNo))
            {
                contracts = contracts.Where(c => c.Code.Contains(contractNo));
            }

            return (await contracts
                .OrderBy(c => c.Code)
                .ToPagedListAsync(pageNumber, pageSize)).ToList();
        }
       

        public async Task<TransactionView> GetActiveContract(string code)
        {
            var contract = await ReferenceContext
                                .Transactions
                                .Include(t => t.Tenant)
                                .Include(v => v.Villa)
                                .Include(v => v.Payments)
                                .SingleOrDefaultAsync(c => c.Code == code);
            return contract;
        }

        public async Task<TransactionView> GetContractViewById(string id)
        {
            var contract = await ReferenceContext
                           .Transactions
                           .Include(t => t.Tenant)
                           .Include(v => v.Villa)
                           .Include(v => v.Payments)
                           .SingleOrDefaultAsync(c => c.Id == id);

            return contract;
        }

        public async Task<Transaction> GetContractById(string id, bool isPaymentIncluded = true)
        {
            if(isPaymentIncluded)
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
    }
}
