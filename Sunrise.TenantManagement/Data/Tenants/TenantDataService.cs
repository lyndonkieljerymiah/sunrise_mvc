using Sunrise.TenantManagement.Model;
using Sunrise.TenantManagement.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Threading.Tasks;
using Utilities.Enum;

namespace Sunrise.TenantManagement.Data.Tenants
{
    public class TenantDataService : ITenantDataService
    {

        private AppDbContext Context { get; set; }

        public TenantDataService(AppDbContext context)
        {
            Context = context;
        }
        
        public async Task<CustomResult> CreateTenant(Tenant tenant,Action<Tenant> callback = null)
        {
            var result = new CustomResult();
            try
            {
                Context.Tenants.Add(tenant);
                await Context.SaveChangesAsync();
                result.Success = true;
                result.ReturnObject = tenant.Id;
                if (callback != null) callback(tenant);
            }
            catch(Exception e)
            {
                result.AddError("TenantInternalErrorException", e.Message);
                result.Success = false;
            }
            return result;
        }

        public async Task<CustomResult> RemoveTenant(string id, Action callback = null)
        {
            var result = new CustomResult();
            try
            {
                var tenant = await Context.Tenants.FindAsync(id);
                Context.Tenants.Remove(tenant);
                await Context.SaveChangesAsync();
                result.Success = true;
                if (callback != null) callback();
            }
            catch (Exception e)
            {
                result.AddError("TenantInternalErrorException", e.Message);
                result.Success = false;
            }
            return result;
        }

        public async Task<Tenant> GetTenant(string id)
        {
            try
            {
                return await Context.Tenants
                                .Include(t => t.Individual)
                                .Include(t => t.Company)
                                .SingleOrDefaultAsync(t => t.Id == id);
            }
            catch(Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }
    }
}
