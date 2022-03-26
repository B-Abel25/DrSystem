using DoctorSystem.Model.Enums;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoctorSystem.Entities
{
    [Table("OfficeHours")]
    public class OfficeHours : AbstractIdentifiable
    {
        public Days Day { get; set; }
        public DateTime Open { get; set; }
        public DateTime Close { get; set; }
        public Doctor Doctor { get; set; }
    }
}
