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

        public TransactionViewModel(IEnumerable<Selection> selections,VillaViewModel villa)
        {
            
            //get for tenant
            var tenantSelections = selections.Where(s => s.Type == "TenantType");
            this.Tenant = new TenantRegisterViewModel(tenantSelections);

            //get for sales
            var salesSelections = selections.Where(s => s.Type.Contains("RentalType") || s.Type.Contains("ContractStatus"));
            this.Sales = SalesViewModel.Create(villa,salesSelections);
        }

        public TransactionViewModel()
        {
                
        }


        public TenantRegisterViewModel Tenant { get; set; }
        public SalesViewModel Sales { get; set; }   
        public string Template { get; set; }

        
    }
}
