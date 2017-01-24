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
            _receivableDataManager = receivableDataManager;
            _villaDataManager = villaDataManager;
        }

        /// <summary>
        /// TODO: Calls when creating receivable
        /// </summary>
        /// <param name="billNo"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{contractCode?}")]
        public async Task<IHttpActionResult> Create(string contractCode)
        {
            var contract = await _contractDataManager.GetContractForPaymentClearing(contractCode);
            if (contract == null)
            {
                ModelState.AddModelError("NotFoundException", "Invalid or no existing contract");
                return BadRequest(ModelState);
            }

            //create new payment for client code use 
            //let the client do the dropdown population
            contract.Initialize((await _selectionDataManager.GetLookup(new string[] { "PaymentStatus" })));
            return Ok(contract);
        }

        /// <summary>
        /// TODO: Update Payment
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("update")]
        public async Task<IHttpActionResult> ClearPayment(ReceivableViewModel vm)
        {
            var userId = User.Identity.GetUserId();

            //update the payment
            //and call hookup to update the villa status
            var result = await _receivableDataManager.ClearPayment(vm.Id, vm.Payments, userId,new Func<string,Task>(UpdateWhenClearingPayment));
            return Ok(result);
        }

        [HttpPost]
        [Route("reverse")]
        public async Task<IHttpActionResult> ReverseContract(ReceivableViewModel vm)
        {
            var result = await _receivableDataManager.ReverseContract(vm.Id, new Func<string, Task>(UpdateWhenContractReserved));
            if (!result.Success)
            {
                var errors = result.Errors;
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                    return BadRequest(ModelState);
                }
            }
            return Ok(result);
        }

        #region Private Method
        public async Task UpdateWhenClearingPayment(string villaId)
        {
            await  _villaDataManager.UpdateVillaStatus(villaId, VillaStatusEnum.NotAvailable);
        }
        public async Task UpdateWhenContractReserved(string villaId)
        {
            await _villaDataManager.UpdateVillaStatus(villaId, VillaStatusEnum.Reserved);
        }
        #endregion

    }
}
