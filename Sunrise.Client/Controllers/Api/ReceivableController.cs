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
            var statusLookup = await _selectionDataManager.GetLookup(new string[] { "PaymentStatus" });
            contract.setPaymentStatuses(statusLookup);

            if (contract == null)
                return BadRequest("Invalid or no exisitng contract");

            return Ok(contract);
        }
        
        [HttpPost]
        [Route("update")]
        public async Task<IHttpActionResult> UpdatePayment(SalesViewModel vm)
        {   
            //update the payment first
            var result = await _contractDataManager.UpdatePayments(vm.Id, vm.Payments);
            if(result.Success)
            {
                await _villaDataManager.UpdateVillaStatus(vm.Villa.Id, VillaStatusEnum.NotAvailable);

            }
            return Ok(result);
        }
    }
}
