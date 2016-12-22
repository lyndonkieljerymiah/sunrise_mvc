using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sunrise.Client.Domains.Models;

namespace Sunrise.Client.Persistence.Abstract
{
    public interface ITenantRepository : IBaseRepository<Tenant>
    {
        Task<IEnumerable<Tenant>> GetTenants();

    }
}
