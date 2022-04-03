using MySql.EntityFrameworkCore.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoctorSystem.Entities
{
    [Table("Appointment")]
    [MySqlCharset("utf8")]
    public class Appointment : AbstractAuditable
    {
        [Column("AppointmentingUser", TypeName = "varchar(37)")]
        [Required]
        public User AppointmentingUser { get; set; }
        [Column("DoctorId", TypeName = "varchar(37)")]
        [Required]
        public Doctor Doctor { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Column("Description", TypeName = "varchar(255)")]
        [Required]
        [MySqlCollation("utf8_hungarian_ci")]
        public string Description { get; set; }
        [Required]
        public bool IsDeleted { get; set; }
    }
}
