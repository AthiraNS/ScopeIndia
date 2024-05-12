using System.ComponentModel.DataAnnotations;

namespace ScopeIndia.Models
{
    public class RegistrationProfile
    {
        public int? Id { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Email { get; set; }

 
        public string? PhoneNumber { get; set; }


        public string? Country { get; set; }

        
        public string? State { get; set; }

 
        public string? City { get; set; }

        public int? CourseId { get; set; }

        public IFormFile? Avatar { get; set; }
        public string? AvatarPath { get; set; }

    





    }
}
