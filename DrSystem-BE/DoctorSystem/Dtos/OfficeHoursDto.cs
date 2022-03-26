using DoctorSystem.Entities;
using DoctorSystem.Model.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace DoctorSystem.Controllers
{
    public class OfficeHoursDto
    {
        [Required]
        public Days Day { get; set; }
        [Required]
        public string Open { get; set; }
        [Required]
        public string Close { get; set; }
        public OfficeHoursDto(OfficeHours oh)
        {
            this.Day = oh.Day;
            this.Open = oh.Open.ToString("HH:mm");
            this.Close = oh.Close.ToString("HH:mm");
           
        }
        public OfficeHoursDto()
        {
        }
    }
}