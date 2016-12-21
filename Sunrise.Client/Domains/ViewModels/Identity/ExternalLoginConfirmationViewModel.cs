using System.ComponentModel.DataAnnotations;

namespace Sunrise.Client.Domains.ViewModels.Identity
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}