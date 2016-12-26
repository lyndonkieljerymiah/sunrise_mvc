using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Sunrise.Client.Domains.Models;
using Sunrise.Client.Domains.ViewModels;
using Sunrise.Client.Persistence.Manager;
using Sunrise.Client.Persistence.Repositories;

namespace Sunrise.Client.Controllers.Api
{
    [RoutePrefix("api/sales")]
    public class SalesController : ApiController
    {
        private readonly SalesDataManager _salesManager;
        private readonly VillaDataManager _villaDataManager;
        private readonly UnitOfWork _unitOfWork;

        public SalesController(
            SalesDataManager salesManager, 
            VillaDataManager villaDataManager,
            UnitOfWork unitOfWork)
        {
            _salesManager = salesManager;
            _villaDataManager = villaDataManager;
            _unitOfWork = unitOfWork;

        }

        [HttpGet]
        [Route("list")]
        public async Task<IHttpActionResult> List()
        {   
            var shop = await  _villaDataManager.GetVillas();
            return Ok(shop);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="villaId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("create/{villaId?}")]
        public async Task<IHttpActionResult> Create(int villaId)
        {

            var villa = await _villaDataManager.GetVilla(villaId);
            var selections = await _unitOfWork.Selections.GetSelections(new string[] { "TenantType", "RentalType", "ContractStatus" });

            var vmTransaction = new TransactionViewModel();
            
            vmTransaction.SetSelections(selections);
            vmTransaction.AddVillaToSales(villa);

            return Ok(vmTransaction);
        }

        /// <summary>
        /// create tenant and save transaction
        /// 
        /// </summary>
        /// <param name="vm"></param>
        /// <returns>Id</returns>
        [HttpPost]
        [Route("create")]
        public async Task<IHttpActionResult> Create(TransactionViewModel vm)
        {   

            try
            {
                if (ModelState.IsValid)
                    return BadRequest();

                var vmTenant = vm.Tenant;
                var vmSales = vm.Sales;
                vmTenant.Code = vm.Sales.VillaNo;

                //register tenant
                var tenant = Tenant.Create(
                    vmTenant.TenantType, vmTenant.Code, vmTenant.Name,
                    vmTenant.EmailAddress, vmTenant.TelNo, vmTenant.
                        MobileNo, vmTenant.FaxNo, vmTenant.Address1, vmTenant.Address2,
                    vmTenant.City, vmTenant.PostalCode);

                if (vmTenant.TenantType == "ttin")
                {
                    tenant.AddIndividual(vmTenant.Individual.Birthday, vmTenant.Individual.Gender,
                        vmTenant.Individual.QatarId, vmTenant.Individual.Company);
                }
                else
                {
                    tenant.AddCompany(vmTenant.Company.CrNo, vmTenant.Company.BusinessType,
                        vmTenant.Company.ValidityDate, vmTenant.Company.Representative);
                }

                //get user id
                var userId = User.Identity.GetUserId();

                //add sales
                SalesTransaction sales = tenant.AddAndReturnTransaction(vm.Sales.Villa.Id, vmSales.RentalType,
                    vmSales.ContractStatus,
                    vmSales.PeriodStart,
                    vmSales.PeriodEnd, vmSales.Amount, userId);

                _unitOfWork.Tenants.Add(tenant);
                await _unitOfWork.SaveChangesAsync();

                SalesViewModel sv = Mapper.Map<SalesViewModel>(sales);
                return Ok(sv);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpGet]
        [Route("billing/{transactionId?}")]
        public async Task<IHttpActionResult> Billing(string transactionId)
        {
            var transaction = await _salesManager.GetSalesAsync(transactionId);

            return Ok(transaction);

        }
    }
}
