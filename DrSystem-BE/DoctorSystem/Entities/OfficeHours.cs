using DoctorSystem.Model.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoctorSystem.Entities
{
    [Table("OfficeHours")]
    public class OfficeHours : AbstractIdentifiable
    {
        [Required]
        public Days Day { get; set; }
        public DateTime Open { get; set; }
        public DateTime Close { get; set; }
        [Column("DoctorId", TypeName = "varchar(37)")]
        [Required]
        public Doctor Doctor { get; set; }
    }
}
