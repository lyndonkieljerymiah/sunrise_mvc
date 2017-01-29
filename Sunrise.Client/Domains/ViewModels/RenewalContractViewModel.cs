using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunrise.Client.Domains.ViewModels
{
    public class RenewalContractViewModel
    {

        public string Id { get; set; }
        public string VillaId { get; set; }
        public string TenantId { get; set; }
        public string TenantName { get; set; }
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }

        public VillaViewModel Villa { get; set; }
        

    }
}
