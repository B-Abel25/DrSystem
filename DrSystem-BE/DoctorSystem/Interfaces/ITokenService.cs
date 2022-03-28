using DoctorSystem.Entities;

namespace DoctorSystem.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(Doctor doctor);
        string CreateToken(Client client);
        string ReadToken(string header);
    }
}
