using System.ComponentModel.DataAnnotations;

namespace ScopeIndia.Models
{
    public class PasswordGeneration
    {
        public bool? RememberMe { get; set; }
        
        public string Email { get; set; }
       

        [Required(ErrorMessage = "Please enter your new password.")]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Please confirm your new password.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm New Password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
