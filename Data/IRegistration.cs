using ScopeIndia.Models;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace ScopeIndia.Data
{
    public interface IRegistration
    {
        void Insert(Registration registration);
        void Update(Registration registration);
        void Delete(string Email);
        Registration GetById(int Id);  //Registration GetByName(string FirstName);
        Registration GetByEmail(string Email);

        void UpdateCourseId(int Id, int CourseId);
        List<Registration> GetAll();


        //void InsertPassword(PasswordGeneration passwordGenereation, Registration registration);


    }
}
