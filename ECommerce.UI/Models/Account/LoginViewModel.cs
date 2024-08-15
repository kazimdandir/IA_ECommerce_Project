using System.ComponentModel.DataAnnotations;

namespace ECommerce.UI.Models.Account
{
    public class LoginViewModel
    {
        //[Display(Name = "Full Name")]
        //public string FullName{ get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "E-Mail")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }
}
