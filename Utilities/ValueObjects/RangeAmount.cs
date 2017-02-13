using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.ValueObjects
{
    public class RangeAmount : ValueObject<RangeAmount>
    {
        public decimal Amount { get; set; }

    }
}
