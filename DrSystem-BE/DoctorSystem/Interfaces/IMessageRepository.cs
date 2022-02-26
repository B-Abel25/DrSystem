using DoctorSystem.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DoctorSystem.Interfaces
{
    public interface IMessageRepository
    {
        void AddMessage(Message message);
        void DeleteMessage(Message message);
        Task<Message> GetMessageAsync(string id);
        Task<List<Message>> GetDoctorMessagesWithClientAsync(Doctor doctor, Client client);
        Task<List<Message>> GetUnreadRecievedMessages(Doctor doctor);
        Task<bool> SaveAllAsync();
        Task<List<Message>> GetDoctorMessagesAsync(Doctor doctor);
    }
}
