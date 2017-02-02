using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunrise.TransactionManagement.Enum
{
    public enum ContractStatusEnum
    {
        Active,
        Pending,
        Completed,
        Cancelled,
        Terminate,
        Official,
        All
    }


    public struct ContractStatusSelection
    {
        public const string Active = "sscn";
        public const string Completed = "ssc";
        public const string Cancelled = "sscc";
        public const string Pending = "ssp";
    }
}
