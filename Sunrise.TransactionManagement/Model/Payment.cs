using Sunrise.TransactionManagement.Model.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.ValueObjects;

namespace Sunrise.TransactionManagement.Model
{
    public class Payment
    {

        public static Payment Create(
            DateTime paymentDate,
            PaymentTypeDictionary paymentType, 
            PaymentModeDictionary paymentMode, 
            string chequeNo,
            string bank, DateTimeRange coveredPeriod,
            decimal amount,
            string remarks,
            string userId)
        {

            var p = new Payment
            {
                PaymentDate = paymentDate,
                PaymentType = paymentType,
                PaymentMode = paymentMode,
                ChequeNo = chequeNo,
                Bank = StatusDictionary.CreateByDefault(bank),
                Period = coveredPeriod,
                Amount = amount,
                Remarks = remarks
            };

            p.LogStamp(userId);
            return p;
        }

        public Payment()
        {
            this.PaymentDate = DateTime.Today;
            this.StatusDate = DateTime.Today;
            this.PaymentType = PaymentTypeDictionary.CreateCheque();
            this.PaymentMode = PaymentModeDictionary.CreatePayment();
            this.Status = PaymentStatusDictionary.CreateReceived();
            this.UpdateStamp = new UpdateStamp();
        }

        public int Id { get; set; }
        public string BillId { get; set; }
        public DateTime PaymentDate { get; set; }
        public PaymentTypeDictionary PaymentType { get; set; }
        public PaymentModeDictionary PaymentMode { get; set; }
       
        public string ChequeNo { get; set; }

        public StatusDictionary Bank { get; set; }
        public DateTimeRange Period { get; set; }
        public decimal Amount { get; set; }

        public PaymentStatusDictionary Status { get; private set; }
        public DateTime? StatusDate { get; private set; }

        public string Remarks { get; set; }
        public bool IsReverse { get; set; }

        #region non persistence
        public bool IsMarkDeleted { get; set; }
        #endregion

        public virtual UpdateStamp UpdateStamp { get; private set; }
        public virtual Bill Bill { get; set; }
        
        #region method
        public void SetStatus(string status,string remarks,string userId)
        {   
            this.Status = new PaymentStatusDictionary(status);
            this.Remarks = remarks;
            this.StatusDate = DateTime.Today;
            this.LogStamp(userId);
        }
        public void LogStamp(string userId)
        {
            this.UpdateStamp.UserId = userId;
            this.UpdateStamp.DateStamp = DateTime.Today;
        }
        #endregion
    }


}
