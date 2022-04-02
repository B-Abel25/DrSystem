﻿using MySql.EntityFrameworkCore.DataAnnotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoctorSystem.Entities
{
    [Table("Client")]
    [MySqlCharset("utf8")]
    public class Client : User
    {
        [StringLength(9, MinimumLength = 9)]
        [Required]
        public string MedNumber { get; set; }
        [Required]
        public bool Member { get; set; }
        [Required]
        public Doctor Doctor { get; set; }
        [Column("MotherName", TypeName = "varchar(30)")]
        [MySqlCollation("utf8_hungarian_ci")]
        [Required]
        public string MotherName { get; set; }
        [Column("BirthPlace", TypeName = "varchar(37)")]
        [Required]
        public City BirthPlace { get; set; }
    }
}
