using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Sunrise.Client.Domains.ViewModels
{
    public class TenantRegisterViewModel
    {

        public TenantRegisterViewModel()
        {
            this.Individual = new IndividualViewModel();
            this.Company = new CompanyViewModel();
            this.Sales = new HashSet<SalesViewModel>();

        }
        public int Id { get; set; }

        public string Type { get; set; }

        public string FullType
        {
            get
            {
                return (this.Type == "in") ? "Individual" : "Company";

            }
        }

        public IEnumerable<SelectListItem> TenantTypes
        {
            get
            {
                return new List<SelectListItem>
                {
                    new SelectListItem() { Text = "Individual", Value= "in",Selected = true},
                    new SelectListItem() { Text = "Company", Value= "co"}
                };
            }
        }

        public string Code { get; set; }
        public string Name { get; set; }
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }
        [Display(Name = "Tel. No.")]
        public string TelNo { get; set; }
        [Display(Name = "Mobile No.")]
        public string MobileNo { get; set; }
        [Display(Name = "Address 1")]
        public string Address1 { get; set; }
        [Display(Name = "Address 2")]
        public string Address2 { get; set; }
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }
        public string City { get; set; }

        public IndividualViewModel Individual { get; set; }

        public CompanyViewModel Company { get; set; }

        public ICollection<SalesViewModel> Sales { get; set; }
        
    }

    public class IndividualViewModel   
    {

        
        public string Gender { get; set; }

        public string FullGender
        {
            get { return (this.Gender == "male") ? "Male" : "Female"; }
        }

        public DateTime Birthday { get; set; }

        public string Company { get; set; }

        public IEnumerable<SelectListItem> Genders
        {
            get
            {
                return new List<SelectListItem>
                {
                    new SelectListItem() { Text = "Male", Value= "male",Selected = true},
                    new SelectListItem() { Text = "Female", Value= "female"}
                };
            }
        }
    }

    public class CompanyViewModel
    {
        [Display(Name = "Business Type")]
        public string BusinessType { get; set; }

        [Display(Name = "CR No")]
        public string CrNo { get; set; }
        [Display(Name = "Validity Date")]
        public DateTime ValidityDate { get; set; }
        public string Representative { get; set; }  
    }
}
