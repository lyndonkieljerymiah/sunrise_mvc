using AutoMapper;
using Sunrise.Client.Domains.ViewModels;
using System;
using System.Threading.Tasks;
using Sunrise.TenantManagement.Abstract;
using Sunrise.TenantManagement.Model;
using Utilities.Enum;

namespace Sunrise.Client.Persistence.Manager
{
    public class TenantDataManager
    {
        private readonly IUnitOfWork _unitOfWork;

        public TenantDataManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CustomResult> CreateAsync(TenantRegisterViewModel vmTenant)
        {
            var result = new CustomResult();

            try
            {
                //register tenant
                var newTenant = Tenant.Map(
                    vmTenant.Name, vmTenant.EmailAddress, 
                    vmTenant.TelNo, vmTenant.MobileNo, 
                    vmTenant.FaxNo, vmTenant.Address1, 
                    vmTenant.Address2, vmTenant.City, vmTenant.PostalCode);
                if(vmTenant.Individual != null)
                {
                    var individual = vmTenant.Individual;
                    newTenant.AddAttributeIndividual(individual.Birthday,individual.Gender, individual.QatarId, individual.Company);
                }
                else
                {
                    var company = vmTenant.Company;
                    newTenant.AddAttributeCompany(company.CrNo, company.BusinessType, company.ValidityDate, company.Representative);
                }
                _unitOfWork.Tenants.Add(newTenant);
                await _unitOfWork.SaveChanges();

                result.ReturnObject = newTenant.Id;
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
