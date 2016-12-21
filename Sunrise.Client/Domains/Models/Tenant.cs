using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunrise.Client.Domains.Models
{
    public class Tenant
    {

        
        public Tenant()
        {
            
        }

        public int Id { get; private set; }
        
        public string Name { get; set; }
        public string Code { get; set; }
        public string TenantType { get; set; }
        public DateTime DateRegistered { get; set; }
        public string EmailAddress { get; set; }
        public string TelNo { get; set; }
        public string MobileNo { get; set; }
        public string FaxNo { get; set; }
        
        public Individual Individual { get; private set; }
        public Company Company { get; private set; }
        public Address Address { get; private set; }

    }
}
