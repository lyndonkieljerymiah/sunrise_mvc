using System.Collections.Generic;
using Sunrise.Maintenance.Model;

namespace Sunrise.Client.Domains.ViewModels
{
    public class TransactionViewModel
    {
        
        public TransactionViewModel()
        {
            //get for tenant
            this.Tenant = new TenantRegisterViewModel();
            this.TransactionRegister = new TransactionRegisterViewModel();
        }
        
        public TenantRegisterViewModel Tenant { get; set; }
        public TransactionRegisterViewModel TransactionRegister { get; set; }

        public string Template { get; set; }

        public void SetSelections(IEnumerable<Selection> selections)
        {
            Tenant.SetTenantTypes(selections);
            TransactionRegister.SetRentalTypes(selections);
            TransactionRegister.SetContractStatuses(selections);
        }

        public void AddVillaToSales(VillaViewModel villa)
        {
            this.TransactionRegister.Villa = villa;
        }
        
    }
}
