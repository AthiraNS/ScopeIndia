using System.ComponentModel.DataAnnotations;

namespace ScopeIndia.Models
{
    public class Login
    {
        [Required(ErrorMessage = "* Mandatory Field")]
        [EmailAddress(ErrorMessage = "EmailAddress is not valid")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "* Mandatory Field")]
        [RegularExpression(@"^(?=.*[a-zA-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{10}$", ErrorMessage = "Password must be 10 characters long and contain at least one letter, one number, and one special character.")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}
