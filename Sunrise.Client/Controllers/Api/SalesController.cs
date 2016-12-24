using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Sunrise.Client.Domains.Models;
using Sunrise.Client.Domains.ViewModels;
using Sunrise.Client.Persistence.Repositories;

namespace Sunrise.Client.Controllers.Api
{
    [RoutePrefix("api/sales")]
    public class SalesController : ApiController
    {
        private readonly UnitOfWork _unitOfWork;

        public SalesController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("create/{villaId?}")]
        public async Task<IHttpActionResult> Create(int villaId)
        {
            var villa = await _unitOfWork.Villas.FindAsync(villaId);
            var selections = await _unitOfWork.Selections.GetSelections(new string[] { "TenantType", "RentalType", "ContractStatus" });

            var vmVilla = VillaViewModel.Create(villa);
            var vmTransaction = new TransactionViewModel(selections, vmVilla);

            return Ok(vmTransaction);
        }


        [HttpPost]
        [Route("update")]
        public async Task<IHttpActionResult> Update(TransactionViewModel vm)
        {
            if (ModelState.IsValid)
                return BadRequest();

            var vmTenant = vm.Tenant;
            var vmSales = vm.Sales;
             
            //register tenant
            var tenant = Tenant.Create(
                vmTenant.Type,vmTenant.Code,vmTenant.Name,
                vmTenant.EmailAddress,vmTenant.TelNo,vmTenant.
                MobileNo,vmTenant.FaxNo,vmTenant.Address1,vmTenant.Address2,
                vmTenant.City,vmTenant.PostalCode);

            if (vmTenant.Type == "ttin")
            {
                tenant.AddIndividual(vmTenant.Individual.Birthday, vmTenant.Individual.Gender,
                    vmTenant.Individual.QatarId, vmTenant.Individual.Company);
            }
            else
            {
                tenant.AddCompany(vmTenant.Company.CrNo,vmTenant.Company.BusinessType,vmTenant.Company.ValidityDate,vmTenant.Company.Representative);
            }

            
            //add sales
            tenant.AddSalesTransaction(vm.Sales.Villa.Id,vmSales.RentalType,
                                        vmSales.ContractStatus,
                                        vmSales.PeriodStart,
                                        vmSales.PeriodEnd,vmSales.Amount);

            _unitOfWork.Tenants.Add(tenant);
            await _unitOfWork.SaveChangesAsync();


            return Ok();
        }
    }
}
