using System;

namespace Sunrise.Client.Domains.Models
{
    public class Company
    {
        public int TenantId { get; set; }


        public string CrNo { get; set; }
        public DateTime ValidityDate { get; set; }
        public string Representative { get; set; }

    }
}