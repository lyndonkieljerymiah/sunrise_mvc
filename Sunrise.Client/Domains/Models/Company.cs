using System;

namespace Sunrise.Client.Domains.Models
{
    public class Company
    {
        public Company(string crNo,string businessType, DateTime validityDate, string representative)
        {
            this.CrNo = crNo;
            this.ValidityDate = validityDate;
            this.BusinessType = businessType;
            this.Representative = representative;
        }

        public int TenantId { get; set; }

        public string BusinessType { get; set; }
        public string CrNo { get; set; }
        public DateTime ValidityDate { get; set; }
        public string Representative { get; set; }

    }
}