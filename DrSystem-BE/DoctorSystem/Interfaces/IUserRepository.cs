using DoctorSystem.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DoctorSystem.Interfaces
{
    public interface IUserRepository
    {
        void Update(User user);

        Task<bool> SaveAllAsync();
        Task<IEnumerable<User>> GetUsersAsync();
        Task<User> GetUserByMedNumberAsync(string medNumber);

    }
}
