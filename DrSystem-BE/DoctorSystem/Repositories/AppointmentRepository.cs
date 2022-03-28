using DoctorSystem.Entities;
using DoctorSystem.Entities.Contexts;
using DoctorSystem.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoctorSystem.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly BaseDbContext _context;

        public AppointmentRepository(BaseDbContext context)
        {
            _context = context;
        }

        public void PutAppointment(Appointment appointment)
        {
            _context.Add(appointment);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<Appointment>> GetAppointmentsByClientAsync(Client client)
        {
            return await _context._appointments.Include(x => x.AppointmentingUser).Where(x => x.AppointmentingUser == client).ToListAsync();
        }

        public async Task<List<Appointment>> GetAppointmentsByDoctorAsync(Doctor doctor)
        {
            return await _context._appointments.Include(x => x.AppointmentingUser).Where(x => x.Doctor == doctor).ToListAsync();
        }

    }
}
