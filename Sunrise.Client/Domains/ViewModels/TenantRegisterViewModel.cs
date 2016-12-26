using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Sunrise.Client.Domains.Enum;
using Sunrise.Client.Domains.Models;

namespace Sunrise.Client.Domains.ViewModels
{
    public class TenantRegisterViewModel
    {

        public static TenantRegisterViewModel CreateDefault()
        {
            var tenant = new TenantRegisterViewModel();
            tenant.TenantType = "ttin";
            return tenant;
        }

        public TenantRegisterViewModel()
        {
            this.Individual = new IndividualViewModel();
            this.Company = new CompanyViewModel();
            this.TenantTypes = new List<SelectListItem>();
        }

        public int Id { get; set; }

        [Required]
        public string TenantType { get; set; }
        public string FullType { get; private set; }
        public IEnumerable<SelectListItem> TenantTypes { get; private set; }

        [Required]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }
        [Required]
        [Display(Name = "Tel. No.")]
        public string TelNo { get; set; }
        [Display(Name = "Mobile No.")]
        public string MobileNo { get; set; }
        [Display(Name = "Fax No.")]
        public string FaxNo { get; set; }

        [Required]
        [Display(Name = "Address 1")]
        public string Address1 { get; set; }
        [Display(Name = "Address 2")]
        public string Address2 { get; set; }
        public string FullAddress { get { return Address1 + " " + Address2 + " " + City; } } 
        [Required]
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }
        [Required]
        public string City { get; set; }
        

        public IndividualViewModel Individual { get; set; }

        public CompanyViewModel Company { get; set; }


        public void SetTenantTypes(IEnumerable<Selection> selections)
        {
            var tenantTypes = selections
                    .Where(s => s.Type == "TenantType")
                    .Select(s => new SelectListItem() { Text = s.Description, Value = s.Code });

            this.TenantTypes = tenantTypes;
        }


    }

    public class IndividualViewModel   
    {

        public IndividualViewModel()
        {
            this.Birthday = DateTime.Today;
        }
        [Required]
        public GenderEnum Gender { get; set; }
        [Required]
        [Display(Name="Qatar Id")]
        public string QatarId { get; set; }
        [Required]
        public DateTime Birthday { get; set; }
        [Required]
        public string Company { get; set; }


        public string FullGender
        {
            get { return (this.Gender == GenderEnum.Male) ? "Male" : "Female"; }
        }

        public IEnumerable<SelectListItem> Genders
        {
            get
            {   
                return new List<SelectListItem>
                {
                    new SelectListItem() { Text = "Male", Value= GenderEnum.Male.ToString() ,Selected = true},
                    new SelectListItem() { Text = "Female", Value= GenderEnum.Male.ToString()}
                };
            }
        }
    }

    public class CompanyViewModel
    {
        public CompanyViewModel()
        {
            this.ValidityDate = DateTime.Today;
        }
        [Required]
        [Display(Name = "Business Type")]
        public string BusinessType { get; set; }
        [Required]
        [Display(Name = "CR No")]
        public string CrNo { get; set; }
        [Required]
        [Display(Name = "Validity Date")]
        public DateTime ValidityDate { get; set; }
        [Required]
        public string Representative { get; set; }  
    }
}
