using Microsoft.AspNet.Identity;
using Sunrise.Client.Domains.ViewModels;
using Sunrise.Client.Persistence.Manager;
using Sunrise.VillaManagement.Data.Villas;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using Utilities.Enum;

namespace Sunrise.Client.Controllers.Api
{
    [RoutePrefix("api/receivable")]
    [Authorize]
    public class ReceivableController : ApiController
    {
        private BillDataManager _billDataManager;
        private SelectionDataManager _selectionDataManager;
        private VillaDataManager _villaDataManager;

        public ReceivableController(
            BillDataManager billDataManager,
            SelectionDataManager selectionDataManager,
            VillaDataManager villaDataManager)
        {
            _selectionDataManager = selectionDataManager;
            _billDataManager = billDataManager;
            _villaDataManager = villaDataManager;
        }

        /// <summary>
        /// TODO: Get Bill 
        /// </summary>
        /// <param name="billCode"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{billCode}")]
        public async Task<IHttpActionResult> GetBill(string billCode)
        {
            var bill = await _billDataManager.GetBillByCode(billCode);

            if (bill == null)
            {
                ModelState.AddModelError("BillInvalidCodeException", "Invalid Bill No");
                return BadRequest(ModelState);
            }
            var selections = await _selectionDataManager
                .GetLookup(new[] { "Bank", "PaymentTerm", "PaymentMode", "PaymentStatus" });

            bill.Initialize(selections);

            return Ok(bill);
        }

        /// <summary>
        /// TODO: Update Payment
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("update")]
        public async Task<IHttpActionResult> Update(BillingViewModel vm)
        {
            var userId = User.Identity.GetUserId();
            //update the payment
            var result = await _billDataManager.Update(vm, userId);
            return Ok(result);
        }
    }
}
