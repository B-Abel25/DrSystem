using DoctorSystem.Entities;
using System;

namespace DoctorSystem.Dtos
{
    public class AppointmentDto
    {
        public string Title { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }

        public AppointmentDto(Appointment app)
        {
            this.Title = app.AppointmentingUser.Name;
            this.Start = app.Date;
            this.End = app.Date.AddMinutes(app.Doctor.Duration);
            this.Description = app.Description;
            this.Color = "red"; 

        }

        public AppointmentDto()
        {
        }
    }
}
