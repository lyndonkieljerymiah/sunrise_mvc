using Microsoft.AspNet.Identity;
using Sunrise.Client.Domains.ViewModels;
using Sunrise.Client.Persistence.Manager;
using Sunrise.VillaManagement.Data.Villas;
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
    [RoutePrefix("api/billing")]
    [Authorize]
    public class BillingController : ApiController
    {
        private BillDataManager _billDataManager;
        private SelectionDataManager _selectionDataManager;
        private TenantDataManager _tenantDataManager;
        private VillaDataManager _villaDataManager;

        public BillingController(
            BillDataManager billDataManager,
            SelectionDataManager selectionDataManager,
            TenantDataManager tenantDataManager,
            VillaDataManager villaDataManager)
        {
            _billDataManager = billDataManager;
            _selectionDataManager = selectionDataManager;

            _tenantDataManager = tenantDataManager;
            _villaDataManager = villaDataManager;
        }


        /// <summary>
        ///     TODO: Show bill
        /// </summary>
        /// <param name="transactionId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{contractId}")]
        public async Task<IHttpActionResult> Create(string contractId)
        {
            //generate bill
            var bill = await _billDataManager.GenerateBill(contractId);
            var selections = await _selectionDataManager
                .GetLookup(new[] { "Bank", "PaymentTerm", "PaymentMode", "PaymentStatus" });
            bill.Initialize(selections);
            return Ok(bill);
        }

        /// <summary>
        ///     TODO: Save Payment
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task<IHttpActionResult> Create(BillingViewModel vm)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //get current user 
            var userId = User.Identity.GetUserId();
            var result = await _billDataManager.Create(vm, userId, new Func<string, Task>(UpdateIfPaymentCleared));
            if (!result.Success)
            {
                AddErrorResult(result);
                return BadRequest(ModelState);
            }
            return Ok(result);

        }
        
        [HttpGet]
        [Route("edit/{billCode}")]
        public async Task<IHttpActionResult> Edit(string billCode)
        {
            var bill = await _billDataManager.GetBillByCode(billCode);
            if (bill == null)
            {
                ModelState.AddModelError("BillNullException", "Bill not found");
                return BadRequest(ModelState);
            }
            return Ok(bill);
        }

        [HttpPost]
        [Route("update/{billCode}")]
        public async Task<IHttpActionResult> Update(BillingViewModel vm)
        {
            var result = await _billDataManager.Update(vm, User.Identity.GetUserId());
            if (result.Success == false)
            {
                AddErrorResult(result);
                return BadRequest(ModelState);
            }
            return Ok(result);
        }


        //[HttpPost]
        //[Route("dismiss")]
        //public async Task<IHttpActionResult> Dismiss(BillingViewModel vm)
        //{
        //    var result = await _contractDataManager.RemoveContract(vm.Id, new Func<string, string, Task>(UpdateWhenContractRemove));
        //    return Ok(result);
        //}

        #region Private Method
        private void AddErrorResult(CustomResult result)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(error.Key, error.Value);
        }

        private async Task UpdateIfPaymentCleared(string id)
        {
            await _villaDataManager.UpdateVillaStatus(id, VillaStatusEnum.NotAvailable);
        }

        private async Task UpdateWhenContractRemove(string tenantId, string villaId)
        {
            //update status
            await _tenantDataManager.RemoveTenant(tenantId);
            await _villaDataManager.UpdateVillaStatus(villaId, VillaStatusEnum.Available);
        }
        #endregion
    }
}
