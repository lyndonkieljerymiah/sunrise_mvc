using AutoMapper;
using Microsoft.AspNet.Identity;

using Sunrise.Client.Domains.ViewModels;
using Sunrise.Client.Persistence.Manager;
using Sunrise.TenantManagement.Model;
using Sunrise.TransactionManagement.Model;

using System.Threading.Tasks;
using System.Web.Http;
using Utilities.Enum;
using Utilities.Helper;

namespace Sunrise.Client.Controllers.Api
{
    [RoutePrefix("api/sales")]
    public class SalesController : ApiController
    {
        private readonly SalesDataManager _salesManager;
        private readonly SelectionDataManager _selectionDataManager;
        private readonly TenantDataManager _tenantDataManager;
        private readonly VillaDataManager _villaDataManager;

        public SalesController(
            SalesDataManager salesManager,
            VillaDataManager villaDataManager,
            SelectionDataManager selectionDataManager,
            TenantDataManager tenantDataManager)
        {
            _salesManager = salesManager;
            _villaDataManager = villaDataManager;
            _selectionDataManager = selectionDataManager;
            _tenantDataManager = tenantDataManager;
        }

        [HttpGet]
        [Route("list")]
        public async Task<IHttpActionResult> List()
        {
            var shop = await _villaDataManager.GetVillas();
            return Ok(shop);
        }



        /// <summary>
        ///     Todo: Create Register and pass to browser
        /// </summary>
        /// <param name="villaId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("create/{villaId?}/{tenantType?}")]
        public async Task<IHttpActionResult> Create(string villaId, string tenantType = "ttin")
        {
            var villa = await _villaDataManager.GetVilla(villaId);
            villa.Images.Add(new ViewImages(1, Url.Content("~/Content/imgs/villa_1_0.jpg"), ""));
            villa.Images.Add(new ViewImages(2, Url.Content("~/Content/imgs/villa_1_1.jpg"), ""));
            villa.Images.Add(new ViewImages(3, Url.Content("~/Content/imgs/villa_1_2.jpg"), ""));
            villa.Images.Add(new ViewImages(4, Url.Content("~/Content/imgs/villa_1_3.jpg"), ""));

            var selections = await _selectionDataManager.GetLookup(new[] {"TenantType", "RentalType", "ContractStatus"});

            var vmRegister = Mapper.Map<TenantRegisterViewModel>(Tenant.CreateNew(tenantType));
            vmRegister.SetTenantTypes(selections);
            var vmTransaction = Mapper.Map<SalesRegisterViewModel>(Transaction.CreateNew(12, villa.RatePerMonth, new MonthRateCalculation()));
            
            vmTransaction.Register = vmRegister;
            vmTransaction.SetContractStatuses(selections);
            vmTransaction.SetRentalTypes(selections);
            vmTransaction.Villa = villa;

            return Ok(vmTransaction);
        }

        /// <summary>
        ///     create tenant and save transaction
        ///     TODO: Take register and save
        /// </summary>
        /// <param name="vm"></param>
        /// <returns>Id</returns>
        [HttpPost]
        [Route("create")]
        public async Task<IHttpActionResult> Create(SalesRegisterViewModel vm)
        {
            if (vm == null)
                ModelState.AddModelError("", "Model cannot be empty");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            vm.UserId = User.Identity.GetUserId();
            var result = await _tenantDataManager.CreateAsync(vm.Register);
            if (!result.Success)
            {
                AddResult(result);
                return BadRequest(ModelState);
            }

            vm.Register.Id = result.ReturnObject.ToString();
            //add transaction
            var transactionResult = await _salesManager.CreateAsync(vm);
            if(!transactionResult.Success)
            {
                AddResult(result);
                return BadRequest(ModelState);
            }

            //update villa status
            await _villaDataManager.UpdateVillaStatus(vm.Villa.Id, VillaStatusEnum.Reserved);
            var sv = new SalesViewModel {Id = (string)transactionResult.ReturnObject};

            return Ok(sv);
        }

        private void AddResult(CustomResult result)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError("", error);
        }

        /// <summary>
        ///     TODO: Show bill
        /// </summary>
        /// <param name="transactionId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("billing/{transactionId?}")]
        public async Task<IHttpActionResult> Billing(string transactionId)
        {
            var transaction = await _salesManager.GetSalesAsync(transactionId);
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
            var selections = await _selectionDataManager.GetLookup(new[] {"Bank", "PaymentTerm", "PaymentMode"});
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

            var result = await _salesManager.AddPaymentAsync(vm);
            if (result.Success)
                await _villaDataManager.UpdateVillaStatus(vm.VillaId, VillaStatusEnum.NotAvailable);
            return Ok(result);
        }
    }
}