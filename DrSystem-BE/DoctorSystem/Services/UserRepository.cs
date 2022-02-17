using DoctorSystem.Entities;
using DoctorSystem.Entities.Contexts;
using DoctorSystem.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoctorSystem.Services
{
    public class UserRepository : IUserRepository
    { private readonly BaseDbContext _context;

        public UserRepository(BaseDbContext context)
        {
            _context = context;
        }

        public async Task<Client> GetUserByMedNumberAsync(string medNumber)
        {
            return await _context._clients.SingleOrDefaultAsync(x => x.MedNumber == medNumber);
        }

        public async Task<IEnumerable<Client>> GetUsersAsync()
        {
            return await _context._clients.ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(Client user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }
    }
}
