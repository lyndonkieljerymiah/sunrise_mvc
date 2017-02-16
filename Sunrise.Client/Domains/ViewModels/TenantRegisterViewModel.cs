using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Utilities.Enum;
using Sunrise.Maintenance.Model;

namespace Sunrise.Client.Domains.ViewModels
{
    public class TenantRegisterViewModel
    {
       
        public TenantRegisterViewModel()
        {  
            this.TenantTypes = new List<SelectListItem>();
        }

        public string Id { get; set; }
        
        [Required]
        public string TenantType { get; set; }

        public string FullType { get; private set; }
        public IEnumerable<SelectListItem> TenantTypes { get; set; }


        [Required]
        public string Name { get; set; }
        public string Code { get; set; }

        [Required]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        
        [Display(Name = "Tel. No.")]
        public string TelNo { get; set; }

        [Required]
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
        public void CreateTenantType()
        {
            if (this.TenantType == "ttin")
            {
                this.Individual = new IndividualViewModel();
            }
            else
            {
                this.Company = new CompanyViewModel();
            }
        }
    }

    public class IndividualViewModel   
    {
        public IndividualViewModel()
        {
            this.Birthday = DateTime.Today;
            this.Gender = GenderEnum.Male;
        }
        
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
                    new SelectListItem() { Text = "Male", Value=  Convert.ToString(GenderEnum.Male.GetHashCode()),Selected = true},
                    new SelectListItem() { Text = "Female", Value=  Convert.ToString(GenderEnum.Female.GetHashCode())}
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
