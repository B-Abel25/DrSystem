using DoctorSystem.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DoctorSystem.Interfaces
{
    public interface IUserRepository
    {
        void Update(Client user);

        Task<bool> SaveAllAsync();
        Task<IEnumerable<Client>> GetUsersAsync();
        Task<Client> GetUserByMedNumberAsync(string medNumber);

    }
}
