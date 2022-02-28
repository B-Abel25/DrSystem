using DoctorSystem.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DoctorSystem.Interfaces
{
    public interface IMessageRepository
    {
        void DeleteMessage(Message message);
        Task<Message> GetMessageAsync(string id);
        Task<List<Message>> GetDoctorMessagesWithClientAsync(Doctor doctor, Client client);
        Task<List<Message>> GetUnreadRecievedMessages(User user);
        Task<bool> SaveAllAsync();
        Task<List<Message>> GetDoctorMessagesAsync(Doctor doctor);
        Task<List<Message>> GetClientMessagesAsync(Client client);
        void UpdateMessage(Message message);
        Task<Message> GetMessageByContentAndDateSentAndUserId(string content, string sentDate, User user);
    }
}
