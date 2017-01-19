using Sunrise.Client.Domains.ViewModels;
using Sunrise.Client.Persistence.Manager;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Utilities.Enum;
using System.Linq;
using System.Web.Http.ModelBinding;
using Sunrise.Client.Infrastructure.Binding;

namespace Sunrise.Client.Controllers.Api
{
    [Authorize]
    [RoutePrefix("api/villa")]
    public class VillaController : ApiController
    {

        private readonly VillaDataManager _villaDataManager;
        private SelectionDataManager _selectionDataManager;

        public VillaController(VillaDataManager villaDataManager,SelectionDataManager selectionDataManager)
        {
            _villaDataManager = villaDataManager;
            _selectionDataManager = selectionDataManager;
        }

        
        [HttpGet]
        [Route("create")]
        public async Task<IHttpActionResult> Create()
        {
            var selections = await _selectionDataManager.GetLookup(new string[] { "RentalType" });
            var vm = new VillaViewModel(selections);
            return Ok(vm);
        }
        
        [HttpGet]
        [Route("edit/{id?}")]
        public async Task<IHttpActionResult> Edit(string id)
        {
            var v = await _villaDataManager.GetVilla(id);
            v.SetLookup(await _selectionDataManager.GetLookup(new string[] { "RentalType" }));
            return Ok(v);
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
            foreach (var villa in villas)
            {
                villa.Images.Add(new ViewImages(1, Url.Content("~/Content/imgs/villa_1_0.jpg"), ""));
            }
            return Ok(villas);
        }

        [HttpPost]
        [Route("save")]
        public async Task<IHttpActionResult> Save([ModelBinder] VillaViewModel vm)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var result = await _villaDataManager.CreateAsync(vm);

                return Ok(result);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
