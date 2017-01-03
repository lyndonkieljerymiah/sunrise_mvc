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

        public static Transaction CreateNew(int defaultMonthPeriod,decimal ratePerMonth,IRateCalculation rateCalculation)
        {
            return new Transaction(defaultMonthPeriod,ratePerMonth, rateCalculation);
        }

        public static Transaction Map(string rentalType,string contractStatus,DateTime periodStart,DateTime periodEnd,
            decimal amountPayable,string villaId,string tenantId,string userId)
        {

            var transaction = new Transaction
            {
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


        internal Transaction(int defaultMonthPeriod,decimal ratePerMonth,IRateCalculation rateCalculation)
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
        }
        public string Id { get; private set; }
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
        
        public void ComputePayableAmount(decimal rate)
        {   
            this.AmountPayable = _rateCalculation.Calculate(rate, this.PeriodStart.Date, this.PeriodEnd.Date);
        }


        public bool AddPayment(
            string paymentType,string paymentMode,string chequeNo,
            string bank,DateTime coveredFrom,DateTime coveredTo,
            decimal amount,string remarks)
        {

            var existingDate = this.Payments.Where(p => p.CoveredPeriodFrom.Date == coveredFrom.Date).ToList();
            if(existingDate.Count == 0)
            {
                var payment = Payment.Map(paymentType, paymentMode, chequeNo, bank, coveredFrom, coveredTo, amount, remarks);
                Payments.Add(payment);
                this.Status = "sscn";
                return true;
            }
            return false;
            
        }



    }
}
