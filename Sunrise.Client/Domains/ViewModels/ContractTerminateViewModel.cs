using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunrise.Client.Domains.ViewModels
{
    public class ContractTerminateViewModel
    {
        public ContractTerminateViewModel()
        {
            Description = "";
        }

        public string Id { get; set; }

        public string ContractId { get; set; }

        [Display(Name="Code")]
        public string ContractCode { get; set; }

        [Required]
        [Display(Name = "Reason")]
        public string Description { get; set; }

        [Required]
        [Display(Name ="Reference No")]
        public string ReferenceNo { get; set; }

        [Required]
        [Display(Name = "Transaction Code")]
        public string PassCode { get; set; }
    }
}
