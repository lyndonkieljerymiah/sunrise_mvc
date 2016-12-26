using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Infrastructure.Language;
using Sunrise.Client.Domains.Enum;

namespace Sunrise.Client.Domains.Models
{
    public class Tenant
    {

        public static Tenant Create(string type, string code, string name, string emailAddress, string telNo,
            string mobileNo,
            string faxNo,string address1,string address2, string city,string postalCode)
        {
            var tenant = new Tenant(type,code,name,emailAddress,telNo,mobileNo,faxNo);
            tenant.Address = new Address(address1, address2, city, postalCode);
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
            this.SalesTransactions = new HashSet<SalesTransaction>();
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

        public virtual Individual Individual { get; private set; }
        public virtual Company Company { get; private set; }
        public virtual Address Address { get; private set; }
        
        public virtual ICollection<SalesTransaction> SalesTransactions { get; private set; }

        public void AddSalesTransaction(int villaId,string rentalType, string contractStatus,
            DateTime periodStart, DateTime periodEnd, decimal amount,string userId)
        {


            var sales = SalesTransaction.CreateTransaction(villaId, rentalType, contractStatus, periodStart, periodEnd, amount,userId);
            SalesTransactions.Add(sales);
            //notification

        }


        public SalesTransaction AddAndReturnTransaction(int villaId, string rentalType, string contractStatus,
            DateTime periodStart, DateTime periodEnd, decimal amount, string userId)
        {
            var sales = SalesTransaction.CreateTransaction(villaId, rentalType, contractStatus, periodStart, periodEnd, amount, userId);
            SalesTransactions.Add(sales);
            return sales;
        }



        public void AddIndividual(DateTime bday,GenderEnum gender,string qatarId,string company)
        {

            this.Individual = new Individual(bday,gender,qatarId,company);
        }
        public void AddCompany(string crNo,string businessType, DateTime validityDate,string representative)
        {

            this.Company = new Company(crNo,businessType,validityDate,representative);
        }


    }
}
