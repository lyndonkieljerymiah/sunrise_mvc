using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunrise.TenantManagement.Model
{
    public class Address
    {
        public Address(string address1, string address2, string city, string postalCode)
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
