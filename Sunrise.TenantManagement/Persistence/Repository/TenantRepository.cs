using Sunrise.TenantManagement.Abstract;
using Sunrise.TenantManagement.Model;
using Utilities.GeneralRepository;

namespace Sunrise.TenantManagement.Persistence.Repository
{
    public class TenantRepository : BaseRepository<Tenant, AppDbContext>, ITenantRepository
    {
        public TenantRepository(AppDbContext context) : base(context)
        {
        }
    }
}
