using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sunrise.Client.Domains.Models;

namespace Sunrise.Client.Domains.ViewModels
{
    public class TransactionViewModel
    {
        
        public TransactionViewModel()
        {
            //get for tenant
            this.Tenant = TenantRegisterViewModel.CreateDefault();
            this.SalesRegister = new SalesRegisterViewModel();
        }

        public TenantRegisterViewModel Tenant { get; set; }
        public SalesRegisterViewModel SalesRegister { get; set; }

        public string Template { get; set; }


        public void SetSelections(IEnumerable<Selection> selections)
        {
            Tenant.SetTenantTypes(selections);
            SalesRegister.SetRentalTypes(selections);
            SalesRegister.SetContractStatuses(selections);
        }

        public void AddVillaToSales(VillaViewModel villa)
        {
            this.SalesRegister.Villa = villa;
        }
        
    }
}
