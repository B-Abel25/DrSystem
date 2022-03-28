using DoctorSystem.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DoctorSystem.Interfaces
{
    public interface IAppointmentRepository
    {
        Task<List<Appointment>> GetAppointmentsByClientAsync(Client client);
        Task<List<Appointment>> GetAppointmentsByDoctorAsync(Doctor doctor);
        void PutAppointment(Appointment appointment);
        Task<bool> SaveAllAsync();
    }
}
