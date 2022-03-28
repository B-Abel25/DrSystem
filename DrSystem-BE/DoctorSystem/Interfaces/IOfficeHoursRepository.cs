using DoctorSystem.Entities;
using DoctorSystem.Model.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DoctorSystem.Interfaces
{
    public interface IOfficeHoursRepository
    {
        public Task<OfficeHours> GetOfficeHoursByDoctorAndDay(Doctor doctor, Days day);
        public Task<List<OfficeHours>> GetOfficeHoursAllDayByDoctor(Doctor doctor);
        void Update(OfficeHours oh);
        Task<bool> SaveAllAsync();
        void RemoveOfficeHour(OfficeHours oh);
    }
}
