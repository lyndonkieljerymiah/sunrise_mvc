using Sunrise.TenantManagement.Abstract;
using Sunrise.TenantManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.GeneralRepository;

namespace Sunrise.TenantManagement.Persistence.Repository
{
    public class TenantRepository : BaseRepository<Tenant, AppContextDb>, ITenantRepository
    {
        public TenantRepository(AppContextDb context) : base(context)
        {
        }
    }
}
