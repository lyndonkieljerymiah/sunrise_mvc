using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Sunrise.Client.Domains.ViewModels;
using Sunrise.Client.Persistence.Abstract;
using Sunrise.Client.Persistence.Manager;

namespace Sunrise.Client.Controllers.Api
{
    [RoutePrefix("api/villa")]
    public class VillaController : ApiController
    {
        private readonly VillaDataManager _villaDataManager;

        public VillaController(VillaDataManager villaDataManager)
        {
            _villaDataManager = villaDataManager;
            
        }

        [HttpGet]
        [Route("")]
        public async Task<IEnumerable<VillaViewModel>> GetVillas()
        {
            var villas = await _villaDataManager.GetAllVillaAsync();
            return villas;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
