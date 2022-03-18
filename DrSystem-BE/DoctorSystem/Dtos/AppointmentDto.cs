using DoctorSystem.Entities;
using System;

namespace DoctorSystem.Dtos
{
    public class AppointmentDto
    {
        public string Name { get; set; }
        public DateTime Date{ get; set; }
        public string Description { get; set; }

        public AppointmentDto(Appointment app)
        {
            Name = app.Client.Name;
            Date = app.Date;
            Description = app.Description;
        }
    }
}
