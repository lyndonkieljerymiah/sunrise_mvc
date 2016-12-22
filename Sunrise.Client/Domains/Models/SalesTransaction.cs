using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunrise.Client.Domains.Models
{
    public class SalesTransaction
    {

        public SalesTransaction(string rentalType,string contractStatus,
            DateTime periodStart,DateTime periodEnd,decimal amount) : this()
        {

            this.RentalType = rentalType;
            this.ContractStatus = contractStatus;
            this.PeriodStart = periodStart;
            this.PeriodEnd = periodEnd;
        }

        public SalesTransaction()
        {
            this.DateCreated = DateTime.Today;
        }

        public int Id { get; set; }

        public DateTime DateCreated { get; set; }

        public string RentalType { get; set; }
        public string ContractStatus { get; set; }
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public Decimal Amount { get; set; }
        
        public string Status { get; set; }
        public int VillaId { get; set; }
        public int TenantId { get; set; }
        public string UserId { get; set; }
        
        public virtual Villa Villa { get; set; }
        public virtual Tenant Tenant { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
