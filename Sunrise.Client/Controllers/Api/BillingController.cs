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
    [RoutePrefix("api/billing")]
    [Authorize]
    public class BillingController : ApiController
    {
        private ContractDataManager _contractDataManager;
        private SelectionDataManager _selectionDataManager;
        private VillaDataManager _villaDataManager;
        private TenantDataManager _tenantDataManager;

        public BillingController(
            ContractDataManager contractDataManager,
            SelectionDataManager selectionDataManager,
            TenantDataManager tenantDataManager,
            VillaDataManager villaDataManager)
        {
            _contractDataManager = contractDataManager;
            _selectionDataManager = selectionDataManager;
            _villaDataManager = villaDataManager;
            _tenantDataManager = tenantDataManager;
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
            var transaction = await _contractDataManager.GetContractById(transactionId);
            var selections = await _selectionDataManager
                .GetLookup(new[] { "Bank", "PaymentTerm", "PaymentMode", "PaymentStatus" });
            transaction.Initialize(selections);

            return Ok(transaction);
        }

        [HttpGet]
        [Route("list")]
        public async Task<IHttpActionResult> List()
        {
            var transactions = await _contractDataManager.GetContracts("ssp");
            return Ok(transactions);
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

            var result = await _contractDataManager.AddPayment(vm,userId,
                () =>
                {
                    _villaDataManager.UpdateVillaStatusNonAsync(vm.Villa.Id, VillaStatusEnum.NotAvailable);
                });

            if (!result.Success)
            {
                AddErrorResult(result);
                return BadRequest(ModelState);
            }
            
            return Ok(vm);
        }

        [HttpPost]
        [Route("dismiss")]
        public async Task<IHttpActionResult> Dismiss(BillingViewModel vm) {

            var result = await _contractDataManager.RemoveContract(vm.Id,(tenantId,villaId)=> {
                    _tenantDataManager.RemoveTenantNonAsync(tenantId);
                    _villaDataManager.UpdateVillaStatusNonAsync(villaId, VillaStatusEnum.Available);
                });

            return Ok(result);
        }
        
        #region Private Method
        private void AddErrorResult(CustomResult result)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(error.Key, error.Value);
        }

        #endregion
    }
}
