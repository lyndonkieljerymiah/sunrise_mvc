using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sunrise.Client.Domains.Models;

namespace Sunrise.Client.Domains.DTO
{
    public class SalesTransactionDTO
    {
        public string Id { get; set; }
        public DateTime DateCreated { get; set; }

        public string RentalType { get; set; }
        public string FullRentalType { get; set; }
        public string ContractStatus { get; set; }
        public string FullContractStatus { get; set; }
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public Decimal Amount { get; set; }
        public string Status { get; set; }

        public int VillaId { get; set; }
        public int TenantId { get; set; }
        public string UserId { get; set; }

        [ForeignKey("VillaId")]
        public virtual VillaDTO Villa { get; set; }

        [ForeignKey("TenantId")]
        public virtual Tenant Tenant { get; set; }


        public virtual ICollection<PaymentDTO> Payments { get; set; }
    }
}
