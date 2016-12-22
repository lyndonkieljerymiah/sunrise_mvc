using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunrise.Client.Domains.Models
{
    public class Tenant
    {
        public static Tenant Create(string type, string code, string name, string emailAddress, string telNo,
            string mobileNo,
            string faxNo,string address1,string address2, string city,string postalCode, Individual individual = null,Company company = null)
        {
            var tenant = new Tenant(type,code,name,emailAddress,telNo,mobileNo,faxNo);
            tenant.Address = new Address(address1, address2, city, postalCode);

            if (type == "ttin")
            {
                tenant.Individual = individual;
            }
            else
            {
                tenant.Company = company;
            }
            return tenant;
        }

        public Tenant(string type, string code, string name, string emailAddress, string telNo, string mobileNo,
            string faxNo) : this()
        {

            this.TenantType = type;
            this.Code = code;
            this.Name = name;
            this.EmailAddress = emailAddress;
            this.TelNo = telNo;
            this.MobileNo = mobileNo;
            this.FaxNo = faxNo;

        }

        public Tenant()
        {
            this.SalesTransaction = new HashSet<SalesTransaction>();
            this.DateRegistered = DateTime.Today;
            this.Individual = null;
            this.Company = null;
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
        public bool IsActive { get; set; }

        public Individual Individual { get; private set; }
        public Company Company { get; private set; }
        public Address Address { get; private set; }

        public ICollection<SalesTransaction> SalesTransaction { get; set; }

        public void AddSalesTransaction()
        {
            
        }

    }
}
