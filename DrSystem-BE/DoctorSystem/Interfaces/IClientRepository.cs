using DoctorSystem.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DoctorSystem.Services
{
    public interface IClientRepository
    {
        Task<Client> GetClientByMedNumberAsync(string medNumber);
        Task<Client> GetClientByEmailTokenAsync(string emailToken);
        void DeleteClient(Client client);
        Task<List<Client>> GetClientsAsync();
        Task<bool> SaveAllAsync();
        void Update(Client user);
        Task<Client> GetClientByIdAsync(string id);
    }
}