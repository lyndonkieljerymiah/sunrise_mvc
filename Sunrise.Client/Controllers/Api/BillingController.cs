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
        private ContractDataManager _contractDataManager;
        private SelectionDataManager _selectionDataManager;
        private TenantDataManager _tenantDataManager;
        private VillaDataManager _villaDataManager;

        public BillingController(
            ContractDataManager contractDataManager,
            SelectionDataManager selectionDataManager,
            TenantDataManager tenantDataManager,
            VillaDataManager villaDataManager)
        {
            _contractDataManager = contractDataManager;
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
        [Route("{transactionId?}")]
        public async Task<IHttpActionResult> Billing(string transactionId)
        {
            var transaction = await _contractDataManager.GetContractForBilling(transactionId);
            var selections = await _selectionDataManager
                .GetLookup(new[] { "Bank", "PaymentTerm", "PaymentMode", "PaymentStatus" });
            transaction.Initialize(selections);

            return Ok(transaction);
        }

        /// <summary>
        ///     TODO: Save Payment
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("save")]
        public async Task<IHttpActionResult> Save(BillingViewModel vm)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            //get current user 
            var userId = User.Identity.GetUserId();
            var result = await _contractDataManager.AddPayment(
                vm, userId, 
                new Func<string,Task>(UpdateIfPaymentCleared));

            if (!result.Success)
            {
                AddErrorResult(result);
                return BadRequest(ModelState);
            }

            return Ok(vm);
        }
        
        [HttpPost]
        [Route("dismiss")]
        public async Task<IHttpActionResult> Dismiss(BillingViewModel vm)
        {
            var result = await _contractDataManager.RemoveContract(vm.Id,new Func<string,string,Task>(UpdateWhenContractRemove));
            return Ok(result);
        }

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

        private async Task UpdateWhenContractRemove(string tenantId,string villaId)
        {
            //update status
            await _tenantDataManager.RemoveTenant(tenantId);
            await _villaDataManager.UpdateVillaStatus(villaId, VillaStatusEnum.Available);
        }
        #endregion
    }
}
