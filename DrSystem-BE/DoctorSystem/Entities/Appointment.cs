﻿using MySql.EntityFrameworkCore.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoctorSystem.Entities
{
    [Table("Appointment")]
    [MySqlCharset("utf8")]
    public class Appointment : AbstractAuditable
    {
        public User AppointmentingUser { get; set; }
        public Doctor Doctor { get; set; }
        public DateTime Date { get; set; }
        [MySqlCollation("utf8_hungarian_ci")]
        [Column("Description", TypeName = "varchar")]
        [StringLength(300)]
        public string Description { get; set; }
        public bool IsDeleted { get; set; }
    }
}
