using System.ComponentModel.DataAnnotations;

namespace ScopeIndia.Models
{
    public class CourseDetails
    {
        public int? CourseId { get; set; }

        public string CourseName { get; set; }

        public string CourseDuration { get; set; }   

        public string CourseFee { get; set; }
    }
}
