using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunrise.TransactionManagement.Model
{
    public class Terminate
    {
        public Terminate()
        {
            DateStamp = DateTime.Today;

        }
        public int Id { get; set; }
        public DateTime DateStamp { get; set; }
        public string Description { get; set; }
        public string ReferenceNo { get; set; }
        public string UserId { get; set; }

        public Contract Transaction { get; set; }

        public bool IsTerminate()
        {
            return false;
        }

    }
}
