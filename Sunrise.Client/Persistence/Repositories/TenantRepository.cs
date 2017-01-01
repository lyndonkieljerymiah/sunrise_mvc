using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Sunrise.Client.Domains.Models;
using Sunrise.Client.Persistence.Abstract;
using Sunrise.Client.Persistence.Context;

namespace Sunrise.Client.Persistence.Repositories
{
    public class TenantRepository : BaseRepository<Tenant>, ITenantRepository
    {
        public TenantRepository(AppDbContext context, ReferenceDbContext referenceDb) : base(context, referenceDb)
        {
            
        }

        public async Task<Tenant> GetTenantById(int id)
        {
            return  await _context.Tenants
                .Include(t => t.Individual)
                .Include(t => t.Company)
                .SingleOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Tenant> GetTenantByItsAttribute(Expression<Func<Tenant, bool>> condition)
        {
            return await _context.Tenants
                .Include(t => t.Individual)
                .Include(t => t.Company)
                .Where(condition)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Tenant>> GetTenants()
        {
            return  await _context.Tenants.ToListAsync();
        }
    }
}
