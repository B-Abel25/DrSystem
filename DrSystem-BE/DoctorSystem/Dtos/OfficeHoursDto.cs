using DoctorSystem.Entities;
using DoctorSystem.Model.Enums;
using System;

namespace DoctorSystem.Controllers
{
    public class OfficeHoursDto
    {
        public Days Day { get; set; }
        public DateTime Opening { get; set; }
        public DateTime Closing { get; set; }
        public bool Closed { get; set; }
        public OfficeHoursDto(OfficeHours oh)
        {
            this.Day = oh.Day;
            this.Opening = oh.Opening;
            this.Closing = oh.Closing;
            this.Closed = oh.Closed;    
        }
    }
}