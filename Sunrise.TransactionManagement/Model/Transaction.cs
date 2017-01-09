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
                Code = code,
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

        #endregion

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

        public virtual ICollection<Payment> Payments { get; set; }



        #region method
        public void ComputePayableAmount(decimal rate)
        {
            this.AmountPayable = _rateCalculation.Calculate(rate, this.PeriodStart.Date, this.PeriodEnd.Date);
        }

        public bool AddPayment(
           DateTime paymentDate, string paymentType, string paymentMode, string chequeNo,
            string bank, DateTime coveredFrom, DateTime coveredTo,
            decimal amount, string remarks)
        {

            //no  existing covered date
            Payment payment = null;

            //do date validation here
            if (!IsPaymentDateCovered(coveredFrom.Date))
            {
                payment = Payment.Map(paymentDate, paymentType, paymentMode, chequeNo, bank, coveredFrom, coveredTo, amount, remarks);
                //payment is cash must be status cleared
                if (paymentType == "ptcs")
                {
                    payment.SetStatus("psc", "");
                }
                Payments.Add(payment);
                return true;
            }
            return false;
        }

        public void UpdatePaymentStatus(int id, string status, string remarks)
        {
            var payment = this.Payments.SingleOrDefault(p => p.Id == id);
            if (payment.Status != "psc")
                payment.SetStatus(status, remarks);
        }
        public void ActivateStatus()
        {
            if (this.Status != "sscn")
                this.Status = "sscn";
        }
        public decimal GetBalanceDue()
        {
            var totalPayment = this.Payments
                .Where(p => p.Status == "psc")
                .Sum(p => p.Amount);
            return AmountPayable - totalPayment;
        }

        public bool IsPaymentDateCovered(DateTime value)
        {
            var existingDate = 0;
            if (Payments.Count > 0)
            {
                existingDate = this.Payments
                    .Where(p => value.Date >= p.CoveredPeriodFrom.Date &&
                            value.Date < p.CoveredPeriodTo.Date).Count();
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
