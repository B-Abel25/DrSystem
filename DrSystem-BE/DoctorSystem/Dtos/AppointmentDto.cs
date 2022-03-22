using DoctorSystem.Entities;
using System;

namespace DoctorSystem.Dtos
{
    public class AppointmentDto
    {
        public string Name { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public string Description { get; set; }

        public AppointmentDto(Appointment app)
        {
            this.Name = app.AppointmentingUser.Name;
            this.DateStart = app.Date;
            this.DateEnd = app.Date.AddMinutes(app.Doctor.Duration);
            this.Description = app.Description;
        }

        public AppointmentDto()
        {
        }
    }
}
