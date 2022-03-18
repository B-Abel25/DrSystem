using MySql.EntityFrameworkCore.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoctorSystem.Entities
{
    [Table("appointment")]
    [MySqlCharset("utf8")]
    public class Appointment : AbstractAuditable
    {
        public Client Client { get; set; }
        public Doctor Doctor { get; set; }
        public DateTime Date { get; set; }
        [MySqlCollation("utf8_hungarian_ci")]
        [Column("Description", TypeName = "varchar")]
        [StringLength(300)]
        public string Description { get; set; }

    }
}
