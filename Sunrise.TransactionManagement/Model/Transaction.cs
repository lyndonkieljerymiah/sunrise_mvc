using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Helper;

namespace Sunrise.TransactionManagement.Model
{
    public class Transaction
    {

        private readonly IRateCalculation _rateCalculation;


        #region Factory
        public static Transaction CreateNew(int defaultMonthPeriod, decimal ratePerMonth, IRateCalculation rateCalculation)
        {
            return new Transaction(defaultMonthPeriod, ratePerMonth, rateCalculation);
        }
        public static Transaction Map(string code, string rentalType, string contractStatus, DateTime periodStart, DateTime periodEnd,
            decimal amountPayable, string villaId, string tenantId, string userId)
        {
            var transaction = new Transaction
            {
                Code = "C" + code + DateTime.Today.Year.ToString(),
                RentalType = rentalType,
                ContractStatus = contractStatus,
                PeriodStart = periodStart,
                PeriodEnd = periodEnd,
                AmountPayable = amountPayable,
                VillaId = villaId,
                TenantId = tenantId,
                UserId = userId
            };

            return transaction;
        }
        #endregion

        internal Transaction(int defaultMonthPeriod, decimal ratePerMonth, IRateCalculation rateCalculation)
        {
            this.RentalType = "rtff";
            this.ContractStatus = "csl";

            this.DateCreated = DateTime.Today;
            //set the default period
            this.PeriodStart = DateTime.Today;
            this.PeriodEnd = this.PeriodStart.AddMonths(defaultMonthPeriod);

            _rateCalculation = rateCalculation;
            this.ComputePayableAmount(ratePerMonth);

        }
        public Transaction()
        {
            this.Id = Guid.NewGuid().ToString();
            this.DateCreated = DateTime.Today;
            this.Status = "ssp";
            this.Payments = new HashSet<Payment>();
        }
        
        public string Id { get; private set; }
        public string Code { get; set; }
        public DateTime DateCreated { get; private set; }

        public string RentalType { get; set; }
        public string ContractStatus { get; set; }

        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public Decimal AmountPayable { get; set; }

        public string Status { get; private set; }

        public string VillaId { get; set; }
        public string TenantId { get; set; }
        public string UserId { get; set; }
        public bool IsReversed { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
        
       

        #region method
        public void ComputePayableAmount(decimal rate)
        {
            this.AmountPayable = _rateCalculation.Calculate(rate, this.PeriodStart.Date, this.PeriodEnd.Date);
        }

        public bool AddPayment(
           DateTime paymentDate, string paymentType, string paymentMode, string chequeNo,
            string bank, DateTime coveredFrom, DateTime coveredTo,
            decimal amount, string remarks,string userId)
        {

            //no  existing covered date
            Payment payment = null;
            //do date validation here

            bool hasCollision = paymentMode == "ptcq" ? IsPaymentDateCovered(coveredFrom.Date) : false;
            if (!hasCollision)
            {
                payment = Payment.Map(paymentDate, paymentType, paymentMode, chequeNo, bank, coveredFrom, coveredTo, amount, remarks,userId);
                //payment is cash must be status cleared
                if (paymentType == "ptcs")
                {
                    payment.SetStatus("psc", "",userId);
                }
                Payments.Add(payment);
                return true;
            }
            return false;
        }

        public bool UpdatePayment(int id,
           DateTime paymentDate, string paymentType, string paymentMode, string chequeNo,
            string bank, DateTime coveredFrom, DateTime coveredTo,
            decimal amount, string remarks, string userId) {
            var payment = Payments.SingleOrDefault(p => p.Id == id);

            bool hasCollision = paymentMode == "ptcq" ? IsPaymentDateCovered(coveredFrom.Date) : false;
            if (!hasCollision)
            {
                payment.PaymentDate = paymentDate;
                payment.PaymentType = paymentType;
                payment.PaymentMode = paymentMode;
                payment.ChequeNo = chequeNo;
                payment.Bank = bank;
                payment.CoveredPeriodFrom = coveredFrom;
                payment.CoveredPeriodTo = coveredTo;
                payment.Amount = amount;
                payment.LogStamp(userId);
                return true;
            }

            return false;
        }

        public void UpdatePaymentStatus(int id, string status, string remarks,string userId)
        {
            var payment = this.Payments.SingleOrDefault(p => p.Id == id);
            if (payment == null)
                throw new Exception("Cannot find payment");
            
            //shouldn't update if it's the same value 
            //means not dirty
            if (payment.Status != status)
                payment.SetStatus(status, remarks,userId);

        }
        public void ActivateStatus()
        {
            if (this.Status != "sscn")
                this.Status = "sscn";
        }
        public bool ReversedContract()
        {
            var clearPaymentCount = this.Payments.Where(p => p.Status == "psc").Count();
            if (clearPaymentCount > 0)
                return false;

            IsReversed = IsReversed ? false : true;
            if(IsReversed)
            {
                if (IsActive())
                    this.Status = "ssp";
            }
            return true;
        }
        public bool IsPaymentDateCovered(DateTime value,int id=0)
        {
            var existingDate = 0;
            if (Payments.Count > 0)
            {
                if (id == 0)
                {
                    existingDate = this.Payments
                        .Where(p => value.Date >= p.CoveredPeriodFrom.Date &&
                                value.Date < p.CoveredPeriodTo.Date && p.PaymentMode == "pmp").Count();
                }
                else
                {
                    //exclude the selected payment
                    existingDate = this.Payments
                        .Where(p => value.Date >= p.CoveredPeriodFrom.Date &&
                                value.Date < p.CoveredPeriodTo.Date && p.PaymentMode == "pmp" && p.Id != id).Count();

                    var ps = this.Payments
                        .Where(p => value.Date >= p.CoveredPeriodFrom.Date &&
                                value.Date < p.CoveredPeriodTo.Date && p.PaymentMode == "pmp" && p.Id != id).ToList();
                }
            }

            return existingDate > 0 ? true : false;
        }
        public bool IsActive()
        {
            if (this.Status == "sscn")
                return true;
            return false;
        }
        #endregion
    }
}
