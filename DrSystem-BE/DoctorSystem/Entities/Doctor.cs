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
        [StringLength(5, MinimumLength = 5)]
        public string SealNumber { get; set; }
        public ICollection<Client> Clients { get; set; }
        [Required]
        public int Duration { get; set; }
    }
}
