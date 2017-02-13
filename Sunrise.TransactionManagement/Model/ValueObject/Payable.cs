using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Helper;
using Utilities.ValueObjects;

namespace Sunrise.TransactionManagement.Model.ValueObject
{
    public class Payable : ValueObject<Payable>
    {

        private IRateCalculation _rateCalculation;

        public static Payable Create(decimal rateAmount,DateTimeRange dateRange)
        {

            return new Payable(rateAmount, dateRange);
        }

        public Payable(decimal rateAmount, DateTimeRange dateRange)
        {
            _rateCalculation = new MonthRateCalculation(rateAmount);
            this.Amount = _rateCalculation.Calculate(rateAmount, dateRange.Start, dateRange.End);
        }


        public Payable(decimal amount)
        {
            this.Amount = amount;
        }

        public Payable()
        {
            this.Amount = 0;
        }

        public decimal Amount { get; set; }

        public decimal GetBalance(decimal totalAmountPaid)
        {
            return this.Amount - totalAmountPaid;
        }


    }
}
