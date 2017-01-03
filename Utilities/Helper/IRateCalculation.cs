using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Helper
{
    public interface IRateCalculation
    {   
        decimal Calculate(decimal rate,DateTime start,DateTime end);
    }
}
