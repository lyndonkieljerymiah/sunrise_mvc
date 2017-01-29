using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunrise.TransactionManagement.DTO
{
    public class TenantView
    {   
        public string Id { get; set; }

        public DateTime DateRegistered { get; set; }

        public string Name { get; set; }
        public string Code { get; set; }
        public string TenantType { get; set; }
        public string EmailAddress { get; set; }
        public string TelNo { get; set; }
        public string MobileNo { get; set; }
        public string FaxNo { get; set; }
        public bool IsActive { get; set; }
        
        public AddressView Address { get; set; }
        public IndividualView Individual { get; set; }
        public CompanyView Company { get; set; }

        
    }

    public class AddressView
    {
        public string Address1 { get; private set; }
        public string Address2 { get; private set; }
        public string City { get; private set; }
        public string PostalCode { get; private set; }

        public override string ToString()
        {
            return Address1 + " " + Address2 + " " + City + " " + PostalCode;
        }
    }


    public class IndividualView
    {
        public DateTime? Birthday { get; set; }
        public int? Gender { get; set; }
        public string QatarId { get; set; }
        public string Company { get; set; }
    }

    public class CompanyView
    {
        public string BusinessType { get; set; }
        public string CrNo { get; set; }
        public DateTime? ValidityDate { get; set; }
        public string Representative { get; set; }
    }
}
