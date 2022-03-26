using DoctorSystem.Model.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace DoctorSystem.Dtos
{
    public class ModifyOfficeHoursDto
    {
        [Required]
        public Days Day { get; set; }
        [Required]
        public string Open { get; set; }
        [Required]
        public string Close { get; set; }

    }
}
