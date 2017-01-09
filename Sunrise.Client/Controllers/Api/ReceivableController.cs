using Sunrise.Client.Domains.ViewModels;
using Sunrise.Client.Persistence.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Utilities.Enum;

namespace Sunrise.Client.Controllers.Api
{
    [RoutePrefix("api/receivable")]
    [Authorize]
    public class ReceivableController : ApiController
    {
        private readonly ContractDataManager _contractDataManager;
        private SelectionDataManager _selectionDataManager;
        private VillaDataManager _villaDataManager;

        public ReceivableController(
            ContractDataManager contractDataManager,
            SelectionDataManager selectionDataManager,
            VillaDataManager villaDataManager)
        {
            _contractDataManager = contractDataManager;
            _selectionDataManager = selectionDataManager;
            _villaDataManager = villaDataManager;
        }

        [HttpGet]
        [Route("create/{villaNo?}")]
        public async Task<IHttpActionResult> Create(string villaNo)
        {   
            var contract = await _contractDataManager.GetContractByCode(villaNo);
            if (contract == null)
            {
                ModelState.AddModelError("NotFoundException", "Invalid or no existing contract");
                return BadRequest(ModelState);
            }

            contract.Initialize((await _selectionDataManager.GetLookup(new string[] { "PaymentStatus" })));
            return Ok(contract);
        }
        
        [HttpPost]
        [Route("update")]
        public async Task<IHttpActionResult> UpdatePayment(BillingViewModel vm)
        {   

            //update the payment first
            var result = await _contractDataManager.UpdateContractPaymentStatus(vm.Id, vm.Payments);
            if(result.Success)
            {
                await _villaDataManager.UpdateVillaStatus(vm.Villa.Id, VillaStatusEnum.NotAvailable);
            }
            return Ok(result);
        }
    }
}
