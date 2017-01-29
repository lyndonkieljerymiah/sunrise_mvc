using Sunrise.TransactionManagement.Model;
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
        
        public string Id { get; private set; }
        public string Code { get; set; }
        public DateTime DateCreated { get; private set; }

        public string RentalType { get; set; }
        public string RentalTypeCode { get; set; }

        public string ContractStatus { get; set; }
        public string ContractStatusCode { get; set; }

        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public Decimal AmountPayable { get; set; }

        public string Status { get; set; }
        public string StatusCode { get; set; }

        public string VillaId { get; set; }
        public string TenantId { get; set; }
        public string UserId { get; set; }

        public bool IsTerminated { get; set; }

        public virtual ICollection<PaymentView> Payments { get; set; }
                
        public virtual TenantView Tenant { get; set; }
        
        public virtual VillaView Villa { get; set; }

        public decimal GetBalanceDue()
        {
            var totalPayment = this.Payments.Sum(p => p.Amount);
            return AmountPayable - totalPayment;
        }


    }
}
