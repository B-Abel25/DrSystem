using DoctorSystem.Entities;
using DoctorSystem.Entities.Contexts;
using DoctorSystem.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DoctorSystem.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly BaseDbContext _context;

        public ClientRepository(BaseDbContext context)
        {
            _context = context;
        }

        public void DeleteClient(Client client)
        {
            _context._clients.Remove(client);
        }

        public async Task<Client> GetClientByEmailTokenAsync(string emailToken)
        {
            return await _context._clients.SingleOrDefaultAsync(x => x.EmailToken == emailToken);
        }

        public async Task<Client> GetClientByMedNumberAsync(string medNumber)
        {
            return await _context._clients.Include(x => x.Place.City.County).Include(x => x.Doctor.Place.City.County).Include(x => x.BirthPlace.County).Include(x => x.BirthPlace).SingleOrDefaultAsync(x => x.MedNumber == medNumber);
        }

        public async Task<Client> GetClientByIdAsync(string id)
        {
            return await _context._clients.Include(x => x.Place.City.County).Include(x => x.Doctor.Place.City.County).Include(x => x.BirthPlace.County).Include(x => x.BirthPlace).SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Client>> GetClientsAsync()
        {
            return await _context._clients.Include(x => x.Place.City.County).Include(x => x.Doctor.Place.City.County).Include(x => x.BirthPlace.County).Include(x => x.BirthPlace).ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(Client client)
        {
            _context._clients.Update(client);
        }
    }
}