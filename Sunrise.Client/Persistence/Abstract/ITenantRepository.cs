using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Ajax.Utilities;
using Sunrise.Client.Domains.Models;

namespace Sunrise.Client.Persistence.Abstract
{
    public interface ITenantRepository : IBaseRepository<Tenant>
    {
        Task<IEnumerable<Tenant>> GetTenants();
        Task<Tenant> GetTenantById(int id);
        Task<Tenant> GetTenantByItsAttribute(Expression<Func<Tenant, bool>> condition);
    }
}
