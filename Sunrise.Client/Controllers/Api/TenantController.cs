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


        /// <summary>
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("list")]
        public ICollection<TenantRegisterViewModel> List()
        {
            var tenants = new List<TenantRegisterViewModel>
            {
                new TenantRegisterViewModel
                {
                    Name = "Arnold Mercado",
                    EmailAddress = "arnold.mercado@hotmail.com"
                }
            };

            return tenants;
        }

        [Route("{code?}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetTenant(string code)
        {
            var tenant = await _tenantDataManager.GetTenantByItsCode(code);
            if (tenant == null)
            {
                ModelState.AddModelError("error","Not Found");
                return BadRequest(ModelState);
            }
            return Ok(tenant);
        }


        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}