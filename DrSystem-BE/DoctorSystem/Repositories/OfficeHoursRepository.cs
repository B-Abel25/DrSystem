using DoctorSystem.Entities;
using DoctorSystem.Entities.Contexts;
using DoctorSystem.Interfaces;
using DoctorSystem.Model.Enums;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoctorSystem.Repositories
{
    public class OfficeHoursRepository : IOfficeHoursRepository
    {
        BaseDbContext _context;
        public OfficeHoursRepository(BaseDbContext context)
        {
            _context = context;
        }

        public async Task<List<OfficeHours>> GetOfficeHoursAllDayByDoctor(Doctor doctor)
        {
            return await _context._officehours.Include(x => x.Doctor.Place.City.County).Where(x => x.Doctor.Id == doctor.Id).ToListAsync();
        }

        public async Task<OfficeHours> GetOfficeHoursByDoctorAndDay(Doctor doctor, Days day)
        {
            return await _context._officehours.Include(x => x.Doctor.Place.City.County).SingleOrDefaultAsync(x => x.Doctor.Id == doctor.Id && x.Day == day);
        }

        public void Update(OfficeHours oh)
        {
            _context._officehours.Update(oh);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void RemoveOfficeHour(OfficeHours oh)
        {
            _context._officehours.Remove(oh);
        }

    }
}
