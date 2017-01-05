using Sunrise.Client.Domains.ViewModels;
using Sunrise.Client.Persistence.Manager;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace Sunrise.Client.Controllers.Api
{
    [Authorize]
    [RoutePrefix("api/villa")]
    public class VillaController : ApiController
    {
        private readonly VillaDataManager _villaDataManager;

        public VillaController(VillaDataManager villaDataManager)
        {
            _villaDataManager = villaDataManager;
            
        }

        [HttpGet]
        [Route("list")]
        public async Task<IHttpActionResult> GetVillas()
        {
            
            var villas = await _villaDataManager.GetVillas();
            return Ok(villas);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
