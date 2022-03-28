using DoctorSystem.Entities;
using DoctorSystem.Entities.Contexts;
using DoctorSystem.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoctorSystem.Services
{
    public class DoctorRepository : IDoctorRepository
    { 
        private readonly BaseDbContext _context;

        public DoctorRepository(BaseDbContext context)
        {
            _context = context;
        }

        public async Task<Doctor> GetDoctorBySealNumberAsync(string sealNumber)
        {
            return await _context._doctors.Include(x => x.Place.City.County).Include(x => x.Clients).SingleOrDefaultAsync(x => x.SealNumber == sealNumber);
        }

        public async Task<Doctor> GetDoctorByEmailTokenAsync(string emailToken)
        {
            return await _context._doctors.SingleOrDefaultAsync(x => x.EmailToken == emailToken);
        }

        public async Task<List<Doctor>> GetDoctorsAsync()
        {
            return await _context._doctors.Include(x => x.Place.City.County).ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(Doctor doctor)
        {
            _context._doctors.Update(doctor);
        }

    }
}
