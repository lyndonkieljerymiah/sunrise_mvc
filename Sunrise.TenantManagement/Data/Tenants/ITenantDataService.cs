using Sunrise.TenantManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Enum;

namespace Sunrise.TenantManagement.Data.Tenants
{
    public interface ITenantDataService
    {
        Task<CustomResult> CreateTenant(Tenant tenant, Action<Tenant> callback = null);

        Task<CustomResult> RemoveTenant(string id, Action callback = null);
    }
}
