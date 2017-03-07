using Sunrise.TransactionManagement.Model;
using Sunrise.TransactionManagement.Persistence.Repository.Abstract;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities.GeneralRepository;
using Sunrise.TransactionManagement.DTO;
using System.Data.SqlClient;
using System.Linq;
using System;
using Sunrise.TransactionManagement.Model.ValueObject;

namespace Sunrise.TransactionManagement.Persistence.Repository.Concrete
{
    public class ContractRepository :
        BaseRepository<Contract, AppDbContext>,
        IContractRepository
    {
        
        public ContractRepository(AppDbContext context) : base(context)
        {
            
        }

        public async Task<IEnumerable<ContractView>> GetActiveContracts()
        {
            var contracts = await _context.Database
                .SqlQuery<ContractView>("dbo.GetAllContracts")
                .ToListAsync();

            return contracts.Where(c => TransactionStatusDictionary.GetActive().Contains(c.ContractStatusCode));
        }
        public async Task<IEnumerable<ContractView>> GetContracts(string[] status = null)
        {   
            var contracts = await _context.Database.SqlQuery<ContractView>("dbo.GetAllContracts").ToListAsync();
            return contracts.Where(c => status.Contains(c.ContractStatusCode));
        }
        public async Task<IEnumerable<ContractView>> GetOfficialContracts()
        {
            var contracts = await _context.Database.SqlQuery<ContractView>("dbo.GetAllContracts").ToListAsync();
            return contracts.Where(c => TransactionStatusDictionary.GetOfficials().Contains(c.ContractStatusCode));
        }
        public async Task<ContractSingleView> GetSingleContract(string id)
        {

            var parameters = new List<SqlParameter>
            {
                new SqlParameter("ContractId",id)
            };

            var contract = await _context.Database
                .SqlQuery<ContractSingleView>("dbo.GetContractById @ContractId", parameters.ToArray())
                .SingleOrDefaultAsync();
            return contract;

        }
        
    }
}
