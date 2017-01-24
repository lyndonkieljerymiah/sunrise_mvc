using Sunrise.TenantManagement.Data.Tenants;
using System;

namespace Sunrise.VillaManagement.Data.Factory
{
    public interface ITenantDataFactory : IDisposable
    {
        ITenantDataService Tenants { get; }
    }
}
