using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunrise.Client.Domains.Models
{
    public class SalesTransaction
    {
        public virtual Villa Villa { get; set; }
        public virtual Tenant Tenant { get; set; }

        public DateTime DateCreated { get; set; }
        public int Status { get; set; }
        public int VillaId { get; set; }
        public int TenantId { get; set; }
        public string UserId { get; set; }

        public ICollection<Payment> Payments { get; set; }

    }
}
