using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunrise.TransactionManagement.DTO
{
    public class ReconcileView
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string BillId { get; set; }
        public string PaymentModeCode { get; set; }
        public string PaymentModeDescription { get; set; }
        public string PaymentTypeCode { get; set; }
        public string PaymentTypeDescription { get; set; }
        public string BankDescription { get; set; }
        public string BankCode { get; set; }
        public string ChequeNo { get; set; }
        public string ReferenceNo { get; set; }
        public decimal DishonouredAmount { get; set; }
        public decimal Amount { get; set; }
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public string Remarks { get; set; }
    }
}
