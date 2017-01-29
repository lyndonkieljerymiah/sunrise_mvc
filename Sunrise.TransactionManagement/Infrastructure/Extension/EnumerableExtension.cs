using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunrise.TransactionManagement.Infrastructure.Extension
{
    public static class EnumerableExtension
    {   
        public static decimal SumIf<TSource>(this IEnumerable<TSource> source,Func<TSource,bool> clause,Func<TSource,decimal> sumExpression)
        {
            var records = source.Where(clause);
            if (records == null)
                return 0;

            return records.Sum(sumExpression);
        }
    }
}
