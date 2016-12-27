using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sunrise.Client.Domains.Enum;
using Sunrise.Client.Domains.Models;
using Sunrise.Client.Domains.ViewModels;
using Sunrise.Client.Persistence.Abstract;

namespace Sunrise.Client.Persistence.Manager
{
    public class TenantDataManager
    {
        private readonly IUnitOfWork _unitOfWork;

        public TenantDataManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<Result> CreateAsync(TenantRegisterViewModel vmTenant,SalesRegisterViewModel salesViewModel)
        {
            var result = new Result();
            try
            {
                //register tenant
                var tenant = Tenant.Create(
                    vmTenant.TenantType, vmTenant.Code, vmTenant.Name,
                    vmTenant.EmailAddress, vmTenant.TelNo, vmTenant.MobileNo, vmTenant.FaxNo, vmTenant.Address1,
                    vmTenant.Address2,
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

                //add sales
                SalesTransaction sales = tenant.AddAndReturnTransaction(salesViewModel.Villa.Id,
                    salesViewModel.RentalType,
                    salesViewModel.ContractStatus,
                    salesViewModel.PeriodStart,
                    salesViewModel.PeriodEnd, salesViewModel.Amount, salesViewModel.UserId);

                _unitOfWork.Tenants.Add(tenant);
                await _unitOfWork.SaveChangesAsync();

                result.ReturnObject = sales.Id;
                result.Success = true;

            }
            catch (Exception e)
            {
                result.Success = false;
                result.Errors.Add(e.Message);
            }

            return result;
        }
    }
}
