using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Sunrise.Client.Domains.Enum;
using Sunrise.Client.Domains.Models;

namespace Sunrise.Client.Domains.ViewModels
{
    public class TenantRegisterViewModel
    {
        private IEnumerable<Selection> _selections;

        public TenantRegisterViewModel(IEnumerable<Selection> selections) : this()
        {
            _selections = selections;
            this.Type = _selections.FirstOrDefault().Code;
        }

        public TenantRegisterViewModel()
        {
            this.Individual = new IndividualViewModel();
            this.Company = new CompanyViewModel();
            this.Sales = new HashSet<SalesViewModel>();

        }
        public int Id { get; set; }

        [Required]
        public string Type { get; set; }

        public string FullType
        {
            get
            {
                var fullType = _selections.SingleOrDefault(t => t.Code == Type);
                return fullType.Description;
            }
        }

        public IEnumerable<SelectListItem> TenantTypes
        {
            get
            {
                var tenantTypes = new List<SelectListItem>();

                foreach (var selection in _selections)
                {
                    tenantTypes.Add(new SelectListItem() { Text = selection.Description, Value = selection.Code});
                }
                return tenantTypes;
            }
        }


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
        [Required]
        [Display(Name = "Address 1")]
        public string Address1 { get; set; }
        [Display(Name = "Address 2")]
        public string Address2 { get; set; }
        [Required]
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }
        [Required]
        public string City { get; set; }

        public IndividualViewModel Individual { get; set; }

        public CompanyViewModel Company { get; set; }

        public ICollection<SalesViewModel> Sales { get; set; }
        
    }

    public class IndividualViewModel   
    {

        public IndividualViewModel()
        {
            this.Birthday = DateTime.Today;
        }
        [Required]
        public GenderEnum Gender { get; set; }

        public string FullGender
        {
            get { return (this.Gender == GenderEnum.Male) ? "Male" : "Female"; }
        }

        [Required]
        public DateTime Birthday { get; set; }
        [Required]
        public string Company { get; set; }

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
