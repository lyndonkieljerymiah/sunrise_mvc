using Sunrise.TransactionManagement.Model.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.ValueObjects;

namespace Sunrise.TransactionManagement.Model
{
    public class Reconcile
    {
        public static Reconcile Create(string chequeNo, string paymentType, string referenceNo, string bank, decimal dishonouredAmount, decimal amount, DateTimeRange coveredPeriod, string remarks)
        {
            return new Reconcile(chequeNo,paymentType, referenceNo,bank, dishonouredAmount, amount, coveredPeriod, remarks);
        }

        public Reconcile(string chequeNo,string paymentType, string referenceNo,string bank,decimal dishonouredAmount,decimal amount,DateTimeRange coveredPeriod,string remarks)
        {
            this.ChequeNo = chequeNo;
            this.PaymentType = new PaymentTypeDictionary(paymentType);
            this.ReferenceNo = referenceNo;
            this.DishonouredAmount = dishonouredAmount;
            this.Bank = StatusDictionary.CreateByDefault(bank);
            this.Amount = amount;
            this.Period = coveredPeriod;
            this.Remarks = remarks;
        }

        internal Reconcile()
        {

        }

        public int Id { get; set; }
        public string BillId { get; set; }
        public DateTime Date { get; set; }
        public PaymentTypeDictionary PaymentType { get; set; }
        public StatusDictionary Bank { get; set; }
        public string ChequeNo { get; set; }
        public string ReferenceNo { get; set; }
        public decimal DishonouredAmount { get; set; }
        public decimal Amount { get; set; }
        public DateTimeRange Period { get; set; }
        public string Remarks { get; set; }
        public bool IsMarkDeleted { get; set; }
        public virtual Bill Bill { get; set; }


    }
}
