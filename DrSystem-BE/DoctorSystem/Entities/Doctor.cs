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
    public class Doctor : AbstractAuditable
    {
        
        [Required]
        [MySqlCollation("utf8_hungarian_ci")]
        public string Name { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        [StringLength(6, MinimumLength = 6)]
        public string SealNumber { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public Place Place { get; set; }
        [Required]
        public string Street { get; set; }
        [Required]
        public string HouseNumber { get; set; }
        [Required]
        public byte[] Password { get; set; }
        [Required]
        public byte[] PasswordSalt { get; set; }
        [Required]
        public ICollection<User> Users { get; set; }
        [Required]
        public string Token { get; set; }

    }
}
