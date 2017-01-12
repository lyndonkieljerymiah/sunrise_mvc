using Microsoft.AspNet.Identity;
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
        private ReceivableDataManager _receivableDataManager;
        private SelectionDataManager _selectionDataManager;
        private VillaDataManager _villaDataManager;

        public ReceivableController(
            ReceivableDataManager receivableDataManager,
            ContractDataManager contractDataManager,
            SelectionDataManager selectionDataManager,
            VillaDataManager villaDataManager)
        {
            _contractDataManager = contractDataManager;
            _selectionDataManager = selectionDataManager;
            _villaDataManager = villaDataManager;
            _receivableDataManager = receivableDataManager;
        }

        [HttpGet]
        [Route("create/{billNo?}")]
        public async Task<IHttpActionResult> Create(string billNo)
        {
            var contract = await _receivableDataManager.GetActiveContract(billNo);

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
        public async Task<IHttpActionResult> ClearPayment(BillingViewModel vm)
        {
            var userId = User.Identity.GetUserId();
            //update the payment first
            var result = await _receivableDataManager.ClearPayment(vm.Id,vm.Payments,userId,(villaId) => {
                _villaDataManager.UpdateVillaStatusNonAsync(villaId, VillaStatusEnum.NotAvailable);
            });
            return Ok(result);
        }
    }
}
