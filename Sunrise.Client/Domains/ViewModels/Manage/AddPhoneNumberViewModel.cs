using System.ComponentModel.DataAnnotations;

namespace Sunrise.Client.Domains.ViewModels.Manage
{
    public class AddPhoneNumberViewModel
    {
        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string Number { get; set; }
    }
}