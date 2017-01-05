using Sunrise.TransactionManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunrise.TransactionManagement.DTO
{
    public class TransactionDTO
    {
        public Transaction Transaction { get; set; }
        public Payment Payment { get; set; }

    }
}
