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

        public async Task<User> GetUserByMedNumberAsync(string medNumber)
        {
            return await _context._users.SingleOrDefaultAsync(x => x.MedNumber == medNumber);
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _context._users.ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }
    }
}
