using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Helper
{
    public class MonthRateCalculation : IRateCalculation
    {
        
        public decimal Calculate(decimal rate, DateTime start, DateTime end)
        {
            var totalDays = (end - start).TotalDays;
            var totalMonth = Convert.ToInt16(totalDays) / 30;
            var total = rate * (totalMonth);

            return total;
        }
    }
}
