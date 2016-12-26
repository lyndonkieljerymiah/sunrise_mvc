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
