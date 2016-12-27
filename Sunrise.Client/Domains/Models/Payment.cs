using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunrise.Client.Domains.Models
{
    public class Payment
    {
        
        public Payment(string term,string chequeNo,decimal amount) : this()
        {
            this.Term = term;
            this.ChequeNo = chequeNo;
            this.Amount = amount;
        }

        internal Payment()
        {
            this.PaymentDate = DateTime.Today;
            this.Status = "psv";
        }

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
        public DateTime? StatusDate { get; set; }
        public string Remarks { get; set; }

        public virtual  SalesTransaction SalesTransaction { get; set; }

        public void ChangeStatus(string status)
        {
            this.Status = status;
        }
        
    }
}
