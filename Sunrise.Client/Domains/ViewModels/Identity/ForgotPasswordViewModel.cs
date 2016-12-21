using System.ComponentModel.DataAnnotations;

namespace Sunrise.Client.Domains.ViewModels.Identity
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
