using System.ComponentModel.DataAnnotations;

namespace ScopeIndia.Models
{
    public class Login
    {
        public bool RememberMe { get; set; }

        [Required(ErrorMessage = "* Mandatory Field")]
        [EmailAddress(ErrorMessage = "EmailAddress is not valid")]
        public string? Email { get; set; }

        
        [DataType(DataType.Password)]
        public string? Password { get; set; }


    }
}
