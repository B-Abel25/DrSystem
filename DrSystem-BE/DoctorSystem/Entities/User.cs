using DoctorSystem.Dtos;
using DoctorSystem.Model;
using MySql.EntityFrameworkCore.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DoctorSystem.Entities
{
    public abstract class User : AbstractAuditable
    {
        [Required]
        [MySqlCollation("utf8_hungarian_ci")]
        public string Name { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
        [Required]
        public byte[] Password { get; set; }
        [Required]
        public byte[] PasswordSalt { get; set; }
        [Required]
        public string EmailToken { get; set; }
        [Required]
        public Place Place { get; set; }
        [Required]
        public string Street { get; set; }
        [Required]
        public string  HouseNumber { get; set; }
        public ICollection<Message> MessagesRecieved { get; set; }
        public ICollection<Message> MessagesSent { get; set; }
    }
}
