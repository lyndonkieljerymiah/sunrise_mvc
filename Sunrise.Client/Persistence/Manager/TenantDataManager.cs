using AutoMapper;
using Sunrise.Client.Domains.ViewModels;
using System;
using System.Threading.Tasks;
using Sunrise.TenantManagement.Abstract;
using Sunrise.TenantManagement.Model;
using Utilities.Enum;
using Sunrise.VillaManagement.Data.Factory;

namespace Sunrise.Client.Persistence.Manager
{
    public class TenantDataManager
    {
        
        private ITenantDataFactory Factory { get; set; }
        public TenantDataManager(ITenantDataFactory factory)
        {
            Factory = factory;
        }

        public async Task<CustomResult> CreateTenant(TenantRegisterViewModel vmTenant)
        {
            //register tenant
            var newTenant = Tenant.Map(
                vmTenant.Name, vmTenant.EmailAddress,
                vmTenant.TelNo, vmTenant.MobileNo,
                vmTenant.FaxNo, vmTenant.Address1,
                vmTenant.Address2, vmTenant.City, vmTenant.PostalCode);
            if (vmTenant.Individual != null)
            {
                var individual = vmTenant.Individual;
                newTenant.AddAttributeIndividual(individual.Birthday, individual.Gender, individual.QatarId, individual.Company);
            }
            else
            {
                var company = vmTenant.Company;
                newTenant.AddAttributeCompany(company.CrNo, company.BusinessType, company.ValidityDate, company.Representative);
            }
            var result = await Factory.Tenants.CreateTenant(newTenant);
            return result;
        }



        public async Task<CustomResult> RemoveTenant(string id)
        {
            var result = await Factory.Tenants.RemoveTenant(id);
            return result;
        }

        public async Task<TenantRegisterViewModel> GetTenant(string id)
        {
            var tenant = await Factory.Tenants.GetTenant(id);
            return Mapper.Map<TenantRegisterViewModel>(tenant);
        }

    }
}
