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
    public class BillingController : ApiController
    {
        private ContractDataManager _contractDataManager;
        private SelectionDataManager _selectionDataManager;
        private VillaDataManager _villaDataManager;

        public BillingController(
            ContractDataManager contractDataManager,
            SelectionDataManager selectionDataManager,
            VillaDataManager villaDataManager)
        {
            _contractDataManager = contractDataManager;
            _selectionDataManager = selectionDataManager;
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
            var transaction = await _contractDataManager.GetContractById(transactionId);
            return Ok(transaction);
        }
        

        /// <summary>
        ///     TODO: Show Payment
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("payment")]
        public async Task<IHttpActionResult> Payment()
        {
            var payment = new PaymentViewModel();
            var selections = await _selectionDataManager
                .GetLookup(new[] { "Bank", "PaymentTerm", "PaymentMode" });

            payment.SetTerms(selections);
            payment.SetMode(selections);
            payment.SetBank(selections);

            return Ok(payment);
        }

        /// <summary>
        ///     TODO: Save Payment
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("payment")]
        public async Task<IHttpActionResult> Payment(PaymentViewModel vm)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _contractDataManager.AddPayment(vm);
            if (!result.Success)
            {
                AddResult(result, "periodError");
                return BadRequest(ModelState);
            }
            return Ok(result);
        }
   

        #region Private Method
        private void AddResult(CustomResult result, string key = "")
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(key, error);
        }
        #endregion
    }
}
