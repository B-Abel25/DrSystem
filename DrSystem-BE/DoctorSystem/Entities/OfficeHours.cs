using DoctorSystem.Model.Enums;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoctorSystem.Entities
{
    [Table("OfficeHours")]
    public class OfficeHours : AbstractIdentifiable
    {
        public Days Day { get; set; }
        public DateTime Opening { get; set; }
        public DateTime Closing { get; set; }
        public bool Closed { get; set; }
        public Doctor Doctor { get; set; }
    }
}
