using System.ComponentModel.DataAnnotations;

namespace ECommerce.UI.Models.Account
{
    public class RegisterViewModel
    {
        [Display(Name = "Full Name")]
        public string FullName { get; set; }
        
        [Required]
        [EmailAddress]
        [Display(Name = "E-Mail")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]

        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
}
