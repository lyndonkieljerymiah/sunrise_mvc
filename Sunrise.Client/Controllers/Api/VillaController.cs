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
using Sunrise.VillaManagement.Data.Villas;
using System.Collections.Generic;
using AutoMapper;
using Sunrise.VillaManagement.Model;

namespace Sunrise.Client.Controllers.Api
{
    [Authorize]
    [RoutePrefix("api/villa")]
    public class VillaController : ApiController
    {


        private SelectionDataManager _selectionDataManager;
        private VillaDataManager _villaDataManager;

        public VillaController(
            SelectionDataManager selectionDataManager,
            VillaDataManager villaDataManager)
        {
            _selectionDataManager = selectionDataManager;
            _villaDataManager = villaDataManager;
        }


        [HttpGet]
        [Route("create")]
        public async Task<IHttpActionResult> Create()
        {
            var selections = await _selectionDataManager.GetLookup(new string[] { "RentalType" });
            var vm = new VillaViewModel(selections);
            vm.DefaultImageUrl = Url.Content("~/Content/imgs/notavailable.png");
            return Ok(vm);
        }

        [HttpGet]
        [Route("edit/{id?}")]
        public async Task<IHttpActionResult> Edit(string id)
        {
            var villa = await _villaDataManager.GetVilla(id);
            var vm = Mapper.Map<VillaViewModel>(villa);
            vm.DefaultImageUrl = Url.Content("~/Content/imgs/notavailable.png");
            vm.SetLookup(await _selectionDataManager.GetLookup(new string[] { "RentalType" }));
            return Ok(vm);
        }

        [HttpGet]
        [Route("list")]
        public async Task<IHttpActionResult> List()
        {
            var parent = await _selectionDataManager.GetLookup(new string[] { "VillaStatus" });
            var vm = await _villaDataManager.GetVillaMasterList("", VillaStatusEnum.All, 1, 100, Url.Content("~/Content/imgs/notavailable.png"));
            var villaMasterFile = new VillaListMasterFile
            {
                ListView = vm
            };

            foreach (var childItem in parent)
            {
                villaMasterFile.Boards.Add(new 
                    VillaStatusBoard {
                    Header = childItem.Description,
                    Total = villaMasterFile.ListView.Where(v => v.StatusCode == childItem.Code).Count()
                });
            }
            return Ok(villaMasterFile);
        }

        [HttpGet]
        [Route("search/{villaNo?}")]
        public async Task<IHttpActionResult> Search(string villaNo = "")
        {   
            var vm = await _villaDataManager.GetVacantVillas(villaNo, 1, 100, Url.Content("~/Content/imgs/notavailable.png"));
            return Ok(vm);
        }

        [HttpPut]
        [Route("update")]
        public async Task<IHttpActionResult> Update([ModelBinder] VillaViewModel vm)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _villaDataManager.UpdateVilla(vm);
            if (!result.Success)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }
                return BadRequest(ModelState);
            }
            return Ok(result);
        }

        [HttpPost]
        [Route("save")]
        public async Task<IHttpActionResult> Save([ModelBinder] VillaViewModel vm)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            CustomResult result = new CustomResult();

            if(string.IsNullOrEmpty(vm.Id))
            {
                result = await _villaDataManager.CreateVilla(vm);
            }
            else
            {
                result = await _villaDataManager.UpdateVilla(vm);
            }
            
            if (!result.Success)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }
                return BadRequest(ModelState);
            }

            return Ok(result);
        }

        protected override void Dispose(bool disposing)
        {
            _villaDataManager.Dispose();
            _selectionDataManager.Dispose();
            base.Dispose(disposing);
        }
    }
}
