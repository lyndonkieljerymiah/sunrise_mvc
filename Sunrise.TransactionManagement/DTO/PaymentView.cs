using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunrise.TransactionManagement.DTO
{
    public class PaymentView
    {
        [Key]
        public int Id { get; set; }
        public string TransactionId { get; set; }
        public string PaymentType { get; set; }
        public string PaymentMode { get; set; }
        public DateTime PaymentDate { get; private set; }
        public string ChequeNo { get; set; }
        public string Bank { get; set; }

        public DateTime CoveredPeriodFrom { get; set; }
        public DateTime CoveredPeriodTo { get; set; }

        public decimal Amount { get; set; }

        public string Status { get; private set; }
        public DateTime? StatusDate { get; private set; }

        public string Remarks { get; set; }

        [ForeignKey("TransactionId")]
        public virtual TransactionView Transaction { get; set; }
    }
}
