using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.ValueObjects;

namespace Sunrise.Client.Domains.ViewModels
{
    public class InquiryViewModel
    {
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }

        public ContractListViewModel ListView { get; set; }

    }

    
}
