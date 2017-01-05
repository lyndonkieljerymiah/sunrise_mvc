using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunrise.TenantManagement.Model
{
    public class Company
    {
        public Company(string tenantId,string crNo, string businessType, DateTime validityDate, string representative) : this()
        {
            this.CrNo = crNo;
            this.ValidityDate = validityDate;
            this.BusinessType = businessType;
            this.Representative = representative;
            this.TenantId = tenantId;
        }

        public Company()
        {
            this.ValidityDate = DateTime.Today;
        }

        public string TenantId { get; set; }

        public string BusinessType { get; set; }
        public string CrNo { get; set; }
        public DateTime ValidityDate { get; set; }
        public string Representative { get; set; }

        public virtual Tenant Tenant { get; set; }
    }
}
