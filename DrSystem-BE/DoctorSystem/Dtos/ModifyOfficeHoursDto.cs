using DoctorSystem.Model.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace DoctorSystem.Dtos
{
    public class ModifyOfficeHoursDto
    {
        [Required]
        public Days day { get; set; }
        [Required]
        public DateTime Opening { get; set; }
        [Required]
        public DateTime Closing { get; set; }
        [Required]
        public bool Closed { get; set; }

    }
}
