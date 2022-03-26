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
        public string Open { get; set; }
        public string Close { get; set; }
        public OfficeHoursDto(OfficeHours oh)
        {
            this.Day = oh.Day;
            if (oh.Open != DateTime.MinValue)
            {
                this.Open = oh.Open.ToString("HH:mm");
            }
            else
            {
                this.Open = "";
            }
            if (oh.Open != DateTime.MinValue)
            {
                this.Close = oh.Close.ToString("HH:mm");
            }
            else
            {
                this.Close = "";
            }
           
        }
        public OfficeHoursDto()
        {
        }
    }
}