using DoctorSystem.Entities;

namespace DoctorSystem.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}
