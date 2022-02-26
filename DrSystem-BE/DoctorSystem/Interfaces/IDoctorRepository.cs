using DoctorSystem.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DoctorSystem.Interfaces
{
    public interface IDoctorRepository
    {
        Task<Doctor> GetDoctorBySealNumberAsync(string sealNumber);
        Task<Doctor> GetDoctorByEmailTokenAsync(string emailToken);
        Task<List<Doctor>> GetDoctorsAsync();
        Task<bool> SaveAllAsync();
        void Update(Doctor doctor);

    }
}
