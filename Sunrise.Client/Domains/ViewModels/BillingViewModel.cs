using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunrise.Client.Domains.ViewModels
{
    public class BillingViewModel
    {
        public BillingViewModel()
        {
            this.Sales = new HashSet<SalesViewModel>();
        }

        public int Id { get; set; }

        public string TenantType { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public ICollection<SalesViewModel> Sales { get; set; }
    }
}
