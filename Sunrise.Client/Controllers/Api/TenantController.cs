using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Sunrise.Client.Domains.ViewModels;
using Sunrise.Client.Persistence.Manager;

namespace Sunrise.Client.Controllers.Api
{
    [Authorize]
    [RoutePrefix("api/tenant")]
    public class TenantController : ApiController
    {
        private readonly TenantDataManager _tenantDataManager;

        public TenantController(TenantDataManager tenantDataManager)
        {
            _tenantDataManager = tenantDataManager;
        }


      
       


        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}