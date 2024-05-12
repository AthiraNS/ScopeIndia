using System.ComponentModel.DataAnnotations;

namespace ScopeIndia.Models
{
    public class Registration
    {
        public int? Id { get; set; }
        [Required(ErrorMessage = "* Mandatory Field")]
        [StringLength(25, MinimumLength = 1, ErrorMessage = "Enter a Valid Name")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "* Mandatory Field")]
        [StringLength(25, MinimumLength = 2, ErrorMessage = "Enter a Valid Name")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "* Mandatory Field")]
        public string? Gender { get; set; }

        [Required(ErrorMessage = "* Mandatory Field")]
        //[DateOnly(ErrorMessage = "PhoneNumber is not valid")]
        public DateOnly? DateOfBirth { get; set; }

        [Required(ErrorMessage = "* Mandatory Field")]
        [EmailAddress(ErrorMessage = "EmailAddress is not valid")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "* Mandatory Field")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Invalid Phone Number")]
        [Phone(ErrorMessage = "PhoneNumber is not valid")]
        public string? MobileNumber { get; set; }

        [Required(ErrorMessage = "* Mandatory Field")]
        public string? Country { get; set; }

        [Required(ErrorMessage = "* Mandatory Field")]
        public string? State { get; set; }

        [Required(ErrorMessage = "* Mandatory Field")]
        public string? City { get; set; }
        [Required(ErrorMessage ="Please select hobbies")]
        [DataType(DataType.Text)]
        public string[]? Hobbieslist { get; set; }
        public string? Hobbies { get; set; }

        public string? Courses { get; set; }

        [Required(ErrorMessage = "Please select a file.")]
        [DataType(DataType.Text)]
        public IFormFile? ImageFile { get; set; }

        public string? ImagePath { get; set; }
        public string? Otp { get; set; }
        public string? Password { get; set; }
        public bool IsVerified { get; set; }

    }
}
