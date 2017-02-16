using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Sunrise.Client.Domains.Models.Identity;
using Sunrise.Client.Domains.ViewModels;
using Sunrise.Client.Infrastructure.Filter;
using Sunrise.Client.Persistence.Manager;
using Sunrise.TenantManagement.Model;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Utilities.Enum;



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
        private readonly UserDataManager _userManager;

        public ContractController(
            ContractDataManager contractDataManager,
            VillaDataManager villaDataManager,
            SelectionDataManager selectionDataManager,
            TenantDataManager tenantDataManager,
            UserDataManager userManager)
        {
            _contractDataManager = contractDataManager;
            _villaDataManager = villaDataManager;
            _selectionDataManager = selectionDataManager;
            _tenantDataManager = tenantDataManager;
            _userManager = userManager;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("official/{search?}")]
        public async Task<IHttpActionResult> Official(string search = "")
        {
            var contracts = await _contractDataManager.GetOfficialContracts(1, 500);
            return Ok(contracts);
        }

        /// <summary>
        /// TODO: Get Contract to be expired 6 months
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("active/{search?}")]
        public async Task<IHttpActionResult> Active(string search = "")
        {
            var contracts = await _contractDataManager.GetActiveContracts(1, 500);
            return Ok(contracts);
        }

        #region renewal
        [HttpGet]
        [Route("renewal/{contractId}")]
        public async Task<IHttpActionResult> Renewal(string contractId)
        {   
            var currentContract = await _contractDataManager.GetContractForRenewal(contractId);
            if (currentContract == null)
            {
                ModelState.AddModelError("ErrorNullException", new Exception("Cannot renew the contract. Please settle payment"));
                return BadRequest(ModelState);
            }

            return Ok(currentContract);
        }
        
        [HttpPost]
        [Route("renewal")]
        public async Task<IHttpActionResult> Renewal(ContractRegisterEditViewModel viewModel)
        {

            var userId = User.Identity.GetUserId();
            var result = await _contractDataManager.Renew(viewModel,userId);
            if (!result.Success)
            {
                AddResult(result);
                return BadRequest(ModelState);
            }
            return Ok(result);
        }
        #endregion

        #region register (get/post/put)
        /// <summary>
        ///     Todo: Create Register and pass to browser
        /// </summary>
        /// <param name="villaId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("register/{villaId?}")]
        public async Task<IHttpActionResult> Register(string villaId)
        {   
            var villaViewModel = await _villaDataManager.GetVilla(villaId);
            villaViewModel.DefaultImageUrl = Url.Content("~/Content/imgs/notavailable.png");

            var selections = await _selectionDataManager.GetLookup(new[] {"TenantType", "RentalType", "ContractStatus"});
            if (villaViewModel.Type == null)
                villaViewModel.Type = "vsav";

            villaViewModel.VillaType = selections.SingleOrDefault(s => s.Code == villaViewModel.Type).Description;
            var vmRegister = Mapper.Map<TenantRegisterViewModel>(Tenant.CreateNew("ttin"));
            vmRegister.SetTenantTypes(selections);
            
            //create and map
            var vmTransaction = _contractDataManager
                .CreateNewContract(villaViewModel.VillaNo,DateTime.Today, villaViewModel.RatePerMonth);

            vmTransaction.VillaId = villaViewModel.Id;
            vmTransaction.RentalType = villaViewModel.Type;
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
        [Route("register")]
        public async Task<IHttpActionResult> Register(ContractRegisterCreateViewModel vm)
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
            return Ok(transactionResult);
        }

        #endregion

        #region remove
        [HttpPost]
        [Route("remove")]
        public async Task<IHttpActionResult> Remove([FromBody]ContractRegisterEditViewModel contract)
        {
            var result = await _contractDataManager.RemoveContract(contract.Id, new Func<string, string, Task>(UpdateWhenContractRemove));
            return Ok(result);
        }
        #endregion
        
        #region terminate
        [HttpGet]
        [Route("terminate/{contractId}")]
        public async Task<IHttpActionResult> Terminate(string contractId)
        {
            var contractForTermination = await _contractDataManager.GetContractForTermination(contractId);
            return Ok(contractForTermination);
        }

        [HttpPost]
        [Route("terminate")]
        public async Task<IHttpActionResult> Terminate(ContractTerminateViewModel vm)
        {

            if(ModelState.IsValid)
            {
                string userId = User.Identity.GetUserId();
                //validation
                if (await _userManager.CheckTransactionCode(userId, vm.PassCode))
                {
                    var result = await _contractDataManager.Terminate(vm.ContractId,
                                    vm.Description,
                                    userId,
                                    new Func<string, Task>(UpdateWhenTermination));
                    return Ok(result);
                }
                else
                {
                    ModelState.AddModelError("validationException", "Invalid Password");
                    return BadRequest(ModelState);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        #endregion

        #region Private Method
        private void AddResult(CustomResult result, string key="")
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(error.Key, error.Value);
        }
        private async Task UpdateWhenContractCreated(string villaId)
        {
            //update status
            await  _villaDataManager.UpdateVillaStatus(villaId, VillaStatusEnum.NotAvailable);
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
        private async Task<ContractRegisterCreateViewModel> UpdateContractProperties(ContractRegisterCreateViewModel contract)
        {
            //get the villa
            var villa = await _villaDataManager.GetVilla(contract.Villa.Id);

            //get tenant
            var tenant = await _tenantDataManager.GetTenant(contract.Register.Id);

            return new ContractRegisterCreateViewModel
            {
                Villa = villa,
                Register = tenant
            };
        }
        private async Task UpdateWhenTermination(string id)
        {
            //reset back 
            await _villaDataManager.UpdateVillaStatus(id, VillaStatusEnum.Available);
        }
        #endregion


    }
}