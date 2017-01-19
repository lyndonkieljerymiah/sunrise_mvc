using Microsoft.AspNet.Identity;
using Sunrise.Client.Domains.ViewModels;
using Sunrise.Client.Persistence.Manager;
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
        /// <summary>
        /// TODO: Calls when creating receivable
        /// </summary>
        /// <param name="billNo"></param>
        /// <returns></returns>
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
            var result = await _receivableDataManager.ClearPayment(vm.Id,vm.Payments,userId,(villaId) => {
                _villaDataManager.UpdateVillaStatusNonAsync(villaId, VillaStatusEnum.NotAvailable);
            });
            return Ok(result);
        }

        [HttpPost]
        [Route("reverse")]
        public async Task<IHttpActionResult> ReverseContract(ReceivableViewModel vm)
        {

            var result = await _receivableDataManager.ReverseContract(vm.Id,(villaId) => {
                _villaDataManager.UpdateVillaStatusNonAsync(villaId, VillaStatusEnum.Reserved);
            });

            if (!result.Success)
            {
                var errors = result.Errors;
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                    return BadRequest(ModelState);
                }
                
            }

            return Ok(result);
        }
        


    }
}
