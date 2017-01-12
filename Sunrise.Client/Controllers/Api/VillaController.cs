using Sunrise.Client.Domains.ViewModels;
using Sunrise.Client.Persistence.Manager;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Utilities.Enum;

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

        [HttpGet]
        [Route("search/{villaNo?}")]
        public async Task<IHttpActionResult> Search(string villaNo="")
        {   
            var villas = await _villaDataManager.GetVillas(villaNo);
            foreach(var villa in villas)
            {
                villa.Images.Add(new ViewImages(1, Url.Content("~/Content/imgs/villa_1_0.jpg"), ""));
            }
            return Ok(villas);
        }


        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
