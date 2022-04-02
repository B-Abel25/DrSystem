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
        [Column("Name", TypeName = "varchar(37)")]
        public string Name { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
        [Required]
        [EmailAddress]
        [Column("Email", TypeName = "varchar(30)")]
        public string Email { get; set; }
        [Required]
        [Phone]
        [Column("PhoneNumber", TypeName = "varchar(13)")]
        public string PhoneNumber { get; set; }
        [Required]
        public byte[] Password { get; set; }
        [Required]
        public byte[] PasswordSalt { get; set; }
        [Column("EmailToken", TypeName = "varchar(40)")]
        [Required]
        public string EmailToken { get; set; }
        [Column("PlaceId", TypeName = "varchar(37)")]
        [Required]
        public Place Place { get; set; }
        [Required]
        [Column("Street", TypeName = "varchar(30)")]
        public string Street { get; set; }
        [Required]
        [Column("HouseNumber", TypeName = "varchar(20)")]
        public string  HouseNumber { get; set; }
        public ICollection<Message> MessagesRecieved { get; set; }
        public ICollection<Message> MessagesSent { get; set; }
    }
}
