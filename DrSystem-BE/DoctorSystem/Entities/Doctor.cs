using DoctorSystem.Model;
using MySql.EntityFrameworkCore.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoctorSystem.Entities
{
    [Table("Doctor")]
    [MySqlCharset("utf8")]
    public class Doctor : User
    {
        [Required]
        [StringLength(6, MinimumLength = 6)]
        public string SealNumber { get; set; }
        [Required]
        public ICollection<Client> Clients { get; set; }
        [Required]
        public int Duration { get; set; }
    }
}
