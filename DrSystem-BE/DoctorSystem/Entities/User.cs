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
    [Table("Users")]
    [MySqlCharset("utf8")]
    public class User : AbstractAuditable
    {

        /*
        Id
	    Name
        DateOfBirth
	    MedNumber
	    Email (not unique)
	    PhoneNumber(not unique)
        PostCode (extend)
        Street
        HouseNumber
        Password
        PasswordSalt (only backend)
        Member
        Token
        DoctorId (extend)
        */
        [Required]
        [MySqlCollation("utf8_hungarian_ci")]
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        [StringLength(9, MinimumLength = 9)]
        [Required]
        public string MedNumber { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public byte[] Password { get; set; }
        [Required]
        public byte[] PasswordSalt { get; set; }
        [Required]
        public bool Member { get; set; }
        [Required]
        public string Token { get; set; }
        [Required]
        public Doctor Doctor{ get; set;}
        [Required]
        public Place Place { get; set; }
        [Required]
        public string Street { get; set; }
        [Required]
        public string  HouseNumber { get; set; }


        
    }
}
