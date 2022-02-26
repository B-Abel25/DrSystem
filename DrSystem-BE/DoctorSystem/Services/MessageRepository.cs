using DoctorSystem.Entities;
using DoctorSystem.Entities.Contexts;
using DoctorSystem.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoctorSystem.Services
{
    public class MessageRepository : IMessageRepository
    {
        BaseDbContext _context;
        public MessageRepository(BaseDbContext context)
        {
            _context = context;
        }
        public void AddMessage(Message message)
        {
            _context._messages.Add(message);
        }

        public void DeleteMessage(Message message)
        {
            _context._messages.Remove(message);
        }

        public async Task<List<Message>> GetDoctorMessagesWithClientAsync(Doctor doctor, Client client)
        {
            return await _context._messages
                 .Include(x => x.Reciever.Place.City.County)
                 .Include(x => x.Sender.Place.City.County)
                 .Where(x => x.Sender.Id == doctor.Id && x.Reciever.Id == client.Id)
                 .OrderBy(x => x.DateSent)
                 .ToListAsync();
        }

        public async Task<List<Message>> GetDoctorMessagesAsync(Doctor doctor)
        {
            return await _context._messages
                 .Include(x => x.Reciever.Place.City.County)
                 .Include(x => x.Sender.Place.City.County)
                 .Where(x => x.Sender.Id == doctor.Id  || x.Reciever.Id == doctor.Id)
                 .OrderBy(x => x.DateSent)
                 .ToListAsync();
        }

        public async Task<Message> GetMessageAsync(string id)
        {
            return await _context._messages
                .Include(x => x.Reciever.Place.City.County)
                .Include(x => x.Sender.Place.City.County).SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Message>> GetUnreadRecievedMessages(Doctor doctor)
        {
            return await _context._messages.Where(m => m.DateRead == null && m.Reciever.Id == doctor.Id).ToListAsync();        
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
