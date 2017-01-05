using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunrise.TransactionManagement.DTO
{
    public class TransactionView
    {
        [Key]
        public string Id { get; private set; }
        public string Code { get; set; }
        public DateTime DateCreated { get; private set; }

        public string RentalType { get; set; }
        public string ContractStatus { get; set; }

        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public Decimal AmountPayable { get; set; }

        public string Status { get; set; }
        public string StatusCode { get; set; }

        public string VillaId { get; set; }
        public string TenantId { get; set; }
        public string UserId { get; set; }

        public virtual ICollection<PaymentView> Payments { get; set; }

        [ForeignKey("TenantId")]    
        public virtual TenantView Tenant { get; set; }

        [ForeignKey("VillaId")]
        public virtual VillaView Villa { get; set; }

        public decimal GetBalanceDue()
        {
            var totalPayment = this.Payments.Sum(p => p.Amount);
            return AmountPayable - totalPayment;
        }
    }
}
