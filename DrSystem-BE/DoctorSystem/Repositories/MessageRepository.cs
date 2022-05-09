using DoctorSystem.Entities;
using DoctorSystem.Entities.Contexts;
using DoctorSystem.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoctorSystem.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        BaseDbContext _context;
        public MessageRepository(BaseDbContext context)
        {
            _context = context;
        }
        public void UpdateMessage(Message message)
        {
            _context._messages.Update(message);
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
                 .Where(x => (x.Sender.Id == doctor.Id && x.Reciever.Id == client.Id) || (x.Sender.Id == client.Id && x.Reciever.Id == doctor.Id))
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

        public async Task<List<Message>> GetUnreadRecievedMessages(User user)
        {
            return await _context._messages
                .Include(x => x.Reciever.Place.City.County)
                .Include(x => x.Sender.Place.City.County)
                .Where(m => m.DateRead == null && m.Reciever.Id == user.Id)
                .ToListAsync();        
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<Message>> GetClientMessagesAsync(Client client)
        {
            return await _context._messages
                .Include(x => x.Reciever.Place.City.County)
                .Include(x => x.Sender.Place.City.County)
                .Where(x => (x.Sender.Id == client.Id && x.Reciever.Id == client.Doctor.Id) || (x.Reciever.Id == client.Id && x.Sender.Id == client.Doctor.Id))
                .OrderBy(x => x.DateSent)
                .ToListAsync();
        }

        public async Task<Message> GetMessageByContentAndDateSentAndUserId(string content, string sentDate, User user)
        {
            return await _context._messages.SingleOrDefaultAsync(x => 
            (x.DateSent.ToString("yyyy.MM.dd HH:mm:ss") == sentDate && x.Content == content) &&
            (x.Sender.Id == user.Id || x.Reciever.Id == user.Id));
        }
    }
}
