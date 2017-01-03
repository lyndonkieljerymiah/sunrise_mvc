using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Enum;

namespace Sunrise.TenantManagement.Model
{
    public class Tenant
    {

        public static Tenant Map(string name, string emailAddress, 
            string telNo, string mobileNo, string faxNo, string address1, 
            string address2, string city, string postalCode)
        {
            return new Tenant(name, emailAddress, telNo, mobileNo, faxNo,address1,address2,city,postalCode);
        }

        public static Tenant CreateNew(string tenantType)
        {
            return new Tenant(tenantType);
            
           
        }

        internal Tenant(string name,string emailAddress,
            string telNo,string mobileNo,string faxNo,string address1,
            string address2,string city,string postalCode) : this()
        {
            this.Name = name;
            this.EmailAddress = emailAddress;
            this.TelNo = telNo;
            this.MobileNo = mobileNo;
            this.FaxNo = faxNo;
            this.Address = new Address(address1, address2, city, postalCode);
        }

        internal Tenant(string tenantType) : this()
        {
            this.TenantType = tenantType;
            if (tenantType == "ttin")
            {
                this.Individual = new Individual();
            }
            else
            {
                this.Company = new Company();
            }
        }

        public Tenant()
        {
            this.Id = Guid.NewGuid().ToString();
            this.DateRegistered = DateTime.Today;
            this.IsActive = true;
        }
        

        public string Id { get; private set; }
        public DateTime DateRegistered { get; private set; }

        public string Name { get; set; }
        public string Code { get; set; }
        public string TenantType { get; set; }
        public string EmailAddress { get; set; }
        public string TelNo { get; set; }
        public string MobileNo { get; set; }
        public string FaxNo { get; set; }
        public bool IsActive { get; set; }

        public virtual Address Address { get; set; }
        public virtual Individual Individual { get; set; }
        public virtual Company Company { get; set; }


        public void AddAttributeCompany(string crNo, string businessType, DateTime validityDate, string representative)
        {
            this.Individual = null;
            this.TenantType = "ttco";
            this.Code = crNo;
            this.Company = new Company(this.Id,crNo, businessType, validityDate, representative);
        }


        public void AddAttributeIndividual(DateTime bday, GenderEnum gender,string qatarId, string company)
        {
            this.Company = null;
            this.TenantType = "ttin";
            this.Code = qatarId;
            this.Individual = new Individual(bday,gender,qatarId,company);
        }

    }
}
