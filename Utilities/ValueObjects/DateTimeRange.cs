using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.ValueObjects
{
    public class DateTimeRange : ValueObject<DateTimeRange>
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }


        public DateTimeRange()
        {

        }


        internal DateTimeRange(DateTime start, DateTime end)
        {
            this.Start = start;
            this.End = end;
        }


        public static DateTimeRange SetRange(int monthDefault)
        {
            var dt = new DateTimeRange();
            dt.Start = DateTime.Today;
            dt.End = dt.Start.AddMonths(monthDefault);
            return dt;
        }
        public static DateTimeRange Create(DateTime start,DateTime end)
        {
            return new DateTimeRange(start, end);
        }
        public static DateTimeRange SetRange(DateTime startDate, int monthDefault)
        {
            var dt = new DateTimeRange();
            dt.Start = startDate;
            dt.End = dt.Start.AddMonths(monthDefault);
            return dt;
        }

        
        public int GetMonthValue(DateTime dateValue)
        {   
            var totalDays = (this.End - DateTime.Today).TotalDays;
            var monthDue = Convert.ToInt16(totalDays) / 30;
            return monthDue;
        }
        
        

        

    }
}
