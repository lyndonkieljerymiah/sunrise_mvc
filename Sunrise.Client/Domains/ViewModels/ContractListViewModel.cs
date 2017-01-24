using System.Collections.Generic;
using Sunrise.Maintenance.Model;
using System;

namespace Sunrise.Client.Domains.ViewModels
{
    public class ContractListViewModel
    {
        
        public string Id { get; set; }
        public DateTime DateCreated { get; set; }
        public string Code { get; set; }
        public string TenantName { get; set; }
        public string VillaNo { get; set; }

        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public string Status { get; set; }
        public string StatusCode { get; set; }
        public decimal AmountPayable { get; set; }
        public decimal CreditAmount { get; set; }
        public decimal AmountBalance { get; set; }
        public bool EditState { get { return StatusCode == "ssp" ? true : false; } }

    }
}
