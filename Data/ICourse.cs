using ScopeIndia.Models;

namespace ScopeIndia.Data
{
    public interface ICourse
    {
      
        CourseDetails GetById(int id);
        List<CourseDetails> GetAll();

    }
}
