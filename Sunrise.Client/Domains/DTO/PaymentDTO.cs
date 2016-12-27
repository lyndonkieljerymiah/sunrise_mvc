using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunrise.Client.Domains.DTO
{
    public class PaymentDTO
    {

        public int Id { get; set; }
        public string SalesTransactionId { get; set; }
        public string Description { get; set; }
        public string Bank { get; set; }
        public string PaymentMode { get; set; }
        public DateTime CoveredPeriodFrom { get; set; }
        public DateTime CoveredPeriodTo { get; set; }
        public string Term { get; set; }
        public string ChequeNo { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; private set; }
        public string PaymentStatus { get; set; }
        public string FullPaymentMode { get; set; } 
        public DateTime? StatusDate { get; set; }
        public string Remarks { get; set; }

        [ForeignKey("SalesTransactionId")]
        public SalesTransactionDTO TransactionDto { get; set; }
         
    }
}
