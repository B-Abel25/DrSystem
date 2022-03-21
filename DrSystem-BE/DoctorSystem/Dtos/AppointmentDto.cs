using DoctorSystem.Entities;
using System;

namespace DoctorSystem.Dtos
{
    public class AppointmentDto
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }

        public AppointmentDto(Appointment app)
        {
            this.Name = app.AppointmentingUser.Name;
            this.Date = app.Date;
            this.Description = app.Description;
        }

        public AppointmentDto()
        {
        }
    }
}
