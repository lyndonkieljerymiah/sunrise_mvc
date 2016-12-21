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


    public class Individual
    {
        public int TenantId { get; set; }

        public DateTime Birthday { get; set; }
        public string Gender { get; set; }
        public string QatarId { get; set; }
        public string Company { get; set; }

    }


    public class Company
    {
        public int TenantId { get; set; }


        public string CrNo { get; set; }
        public DateTime ValidityDate { get; set; }
        public string Representative { get; set; }

    }

    public class Address
    {
        public Address(string address1,string address2,string city,string postalCode)
        {
            this.Address1 = address1;
            this.Address2 = address2;
            this.City = city;
            this.PostalCode = postalCode;
        }

        internal Address()
        {
            
        }
        public string Address1 { get; private set; }

        public string Address2 { get; private set; }

        public string City { get; private set; }

        public string PostalCode { get; private set; }

    }
}
