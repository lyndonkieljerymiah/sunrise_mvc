using AutoMapper;
using Microsoft.AspNet.Identity;

using Sunrise.Client.Domains.ViewModels;
using Sunrise.Client.Persistence.Manager;
using Sunrise.TenantManagement.Model;
using Sunrise.TransactionManagement.Model;
using Sunrise.VillaManagement.Data.Villas;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using Utilities.Enum;
using Utilities.Helper;

namespace Sunrise.Client.Controllers.Api
{
    [RoutePrefix("api/contract")]
    [Authorize]
    public class ContractController : ApiController
    {
        private readonly ContractDataManager _contractDataManager;
        private readonly SelectionDataManager _selectionDataManager;
        private readonly TenantDataManager _tenantDataManager;
        private VillaDataManager _villaDataManager;

        public ContractController(
            ContractDataManager contractDataManager,
            VillaDataManager villaDataManager,
            SelectionDataManager selectionDataManager,
            TenantDataManager tenantDataManager)
        {
            _contractDataManager = contractDataManager;
            _villaDataManager = villaDataManager;
            _selectionDataManager = selectionDataManager;
            _tenantDataManager = tenantDataManager;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("list")]
        public async Task<IHttpActionResult> List()
        {
            var contracts = await _contractDataManager.GetContracts("",1,100);
            return Ok(contracts);
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
            var villaViewModel = await _villaDataManager.GetVilla(villaId);
            villaViewModel.DefaultImageUrl = Url.Content("~/Content/imgs/notavailable.png");

            var selections = await _selectionDataManager.GetLookup(new[] {"TenantType", "RentalType", "ContractStatus"});

            //create and map
            var vmRegister = Mapper.Map<TenantRegisterViewModel>(Tenant.CreateNew(tenantType));
            vmRegister.SetTenantTypes(selections);
            var vmTransaction = Mapper.Map<TransactionRegisterViewModel>(Transaction.CreateNew(12, villaViewModel.RatePerMonth, new MonthRateCalculation()));
            
            vmTransaction.Register = vmRegister;
            vmTransaction.SetContractStatuses(selections);
            vmTransaction.SetRentalTypes(selections);
            vmTransaction.Villa = villaViewModel;

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
        public async Task<IHttpActionResult> Create(TransactionRegisterViewModel vm)
        {
            if (vm == null)
                ModelState.AddModelError("", "Model cannot be empty");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            vm.UserId = User.Identity.GetUserId();
            var result = await _tenantDataManager.CreateTenant(vm.Register);
            if (!result.Success)
            {
                AddResult(result);
                return BadRequest(ModelState);
            }

            vm.Register.Id = result.ReturnObject.ToString();
            
            //add transaction
            var transactionResult = await _contractDataManager.AddContract(vm,new Func<string,Task>(UpdateWhenContractCreated));
            if(!transactionResult.Success)
            {
                AddResult(result);
                return BadRequest(ModelState);
            }
            var sv = new BillingViewModel { Id = (string)transactionResult.ReturnObject};
            return Ok(sv);
        }
        
       

        /// <summary>
        ///     TODO: Save Payment
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        public async Task<IHttpActionResult> Update(BillingViewModel vm)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            //get current user 
            var userId = User.Identity.GetUserId();
            var result = await _contractDataManager.AddPayment(
                vm, userId,
                new Func<string, Task>(UpdateWhenPaymentDone));

            if (!result.Success)
            {
                AddResult(result);
                return BadRequest(ModelState);
            }

            return Ok(vm);
        }
        
        [HttpPost]
        [Route("cancel")]
        public async Task<IHttpActionResult> Cancel(BillingViewModel mv)
        {
            var result = await _contractDataManager.RemoveContract(mv.Id,new Func<string,string,Task>(UpdateWhenContractRemove));

            return Ok(result);
        }

        #region Private Method
        private void AddResult(CustomResult result, string key="")
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(error.Key, error.Value);
        }

        private async Task UpdateWhenContractCreated(string villaId)
        {
            //update status
            await  _villaDataManager.UpdateVillaStatus(villaId, VillaStatusEnum.Reserved);
        }
        private async Task UpdateWhenContractRemove(string tenantId,string villaId)
        {
            //update status
            await _tenantDataManager.RemoveTenant(tenantId);
            await _villaDataManager.UpdateVillaStatus(villaId, VillaStatusEnum.Available);
        }
        private async Task UpdateWhenPaymentDone(string id)
        {
            await _villaDataManager.UpdateVillaStatus(id, VillaStatusEnum.NotAvailable);
        }
        #endregion


    }
}