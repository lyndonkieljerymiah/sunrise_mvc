using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunrise.TransactionManagement.Enum
{
    public enum PaymentStatusEnum
    {
        Clear,
        Hold,
        Bounce,
        Received
    }

    public static class PaymentStatusSelection
    {
        public const string Clear = "psc";
        public const string Bounce = "psb";
        public const string Hold = "psh";
        public const string Received = "psv";

    }
}
