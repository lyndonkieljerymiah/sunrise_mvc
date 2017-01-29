using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Helper;

namespace Sunrise.TransactionManagement.Model
{
    public abstract class TransactionBase
    {
        protected IRateCalculation _rateCalculation;

        public string Id { get; protected set; }
        public string Code { get; protected set; }
        public DateTime DateCreated { get; set; }

        public string RentalType { get; set; }
        public string ContractStatus { get; set; }

        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public Decimal AmountPayable { get; set; }

        public string Status { get; private set; }

        public string VillaId { get; set; }
        public string TenantId { get; set; }
        public string UserId { get; set; }
        public bool IsReversed { get; private set; }
        public bool IsTerminated { get; private set; }

        public virtual void SetNewPeriod(DateTime startPeriod, int defaultMonthPeriod, decimal ratePerMonth, IRateCalculation rateCalculation)
        {
            this.PeriodStart = startPeriod;
            this.PeriodEnd = startPeriod.AddMonths(defaultMonthPeriod);
            this._rateCalculation = rateCalculation;
            this.ComputePayableAmount(ratePerMonth);

        }

        public virtual void ComputePayableAmount(decimal rate)
        {
            this.AmountPayable = _rateCalculation.Calculate(rate, this.PeriodStart.Date, this.PeriodEnd.Date);
        }


    }
}
