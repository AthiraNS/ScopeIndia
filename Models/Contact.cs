using System.ComponentModel.DataAnnotations;

namespace ScopeIndia.Models
{
    public class Contact
    {
        [Required(ErrorMessage = "* Mandatory Field")]
        [StringLength(25, MinimumLength = 2, ErrorMessage = "Enter a Valid Name")]
        public string? FullName { get; set; }

        [Required(ErrorMessage = "* Mandatory Field")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Invalid Phone Number")]
        [Phone(ErrorMessage = "PhoneNumber is not valid")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "* Mandatory Field")]
        [EmailAddress(ErrorMessage = "EmailAddress is not valid")]
        public string? EmailAddress { get; set; }



    }
}
