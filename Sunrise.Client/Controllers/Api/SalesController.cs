using System;
using System.Data.Entity.Core.Metadata.Edm;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Sunrise.Client.Domains.Enum;
using Sunrise.Client.Domains.Models;
using Sunrise.Client.Domains.ViewModels;
using Sunrise.Client.Persistence.Manager;

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
        [Route("create/{villaId?}")]
        public async Task<IHttpActionResult> Create(int villaId)
        {
            var villa = await _villaDataManager.GetVilla(villaId);
            var selections = await _selectionDataManager.GetLookup(new[] {"TenantType", "RentalType", "ContractStatus"});

            var vmRegister = TenantRegisterViewModel.CreateDefault();
            vmRegister.SetTenantTypes(selections);

            var vmTransaction = SalesRegisterViewModel.CreateWithVilla(villa);
            vmTransaction.Register = vmRegister;
            vmTransaction.SetContractStatuses(selections);
            vmTransaction.SetRentalTypes(selections);

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
            try
            {
                if (ModelState.IsValid)
                    return BadRequest();

                vm.UserId = User.Identity.GetUserId();
                var result = await _tenantDataManager.CreateAsync(vm.Register, vm);
                if (result.Success)
                {
                    //update villa status
                    await _villaDataManager.UpdateVillaStatus(vm.Villa.Id, VillaStatusEnum.Reserved);
                    var sv = new SalesViewModel {Id = (string) result.ReturnObject};
                    return Ok(sv);
                }
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
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
            var selections = await _selectionDataManager.GetLookup(new[] {"PaymentTerm", "PaymentMode"});

            payment.SetTerms(selections);
            payment.SetMode(selections);
            payment.Term = "ptcq";
            payment.PaymentMode = "pmp";

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
            var result = await _salesManager.AddPaymentAsync(vm);
            if (result.Success)
            {
                await _villaDataManager.UpdateVillaStatus(vm.VillaId, VillaStatusEnum.NotAvailable);
            }
            return Ok(result);
        }

    }
}