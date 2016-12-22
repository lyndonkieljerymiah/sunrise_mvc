using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Sunrise.Client.Domains.ViewModels;
using Sunrise.Client.Persistence.Abstract;

namespace Sunrise.Client.Controllers.Api
{
    [RoutePrefix("api/villa")]
    public class VillaController : ApiController
    {
        private readonly IUnitOfWork _uw;

        public VillaController(IUnitOfWork uw)
        {
            _uw = uw;
        }

        [HttpGet]
        [Route("{villaNo?}")]
        public async Task<IHttpActionResult> Availability(string villaNo)
        {   
            var villa = await _uw.Villas.GetVilla(villaNo);
            if (villa == null)
                return BadRequest("Not available");

            return Ok(VillaViewModel.Create(villa));
        }

        [HttpGet]
        [Route("")]
        public async Task<IEnumerable<VillaViewModel>> GetVillas()
        {
            var villas = await _uw.Villas.GetAllVilla();
            return VillaViewModel.CreateRange(villas);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _uw.Dispose();
        }
    }
}
