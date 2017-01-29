using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Helper
{
    public class MonthRateCalculation : IRateCalculation
    {
        private decimal _rate;
        public MonthRateCalculation(decimal rate)
        {
            _rate = rate;
        }

        public MonthRateCalculation()
        {
            _rate = 0;
        }

        public decimal Calculate(decimal rate,DateTime start, DateTime end)
        {
            var totalDays = (end - start).TotalDays;
            var totalMonth = Convert.ToInt16(totalDays) / 30;
            if (_rate == 0) _rate = rate;
            var total = _rate * (totalMonth);

            return total;
        }
    }
}
