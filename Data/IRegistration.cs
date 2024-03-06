using ScopeIndia.Models;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace ScopeIndia.Data
{
    public interface IRegistration
    {
        void Insert(Registration registration);
        void Update(Registration registration);
        void Delete(string Email);
        Registration GetByEmail(string Email);
        Registration GetByName(string FirstName);

        List<Registration> GetAll();
    }
}
