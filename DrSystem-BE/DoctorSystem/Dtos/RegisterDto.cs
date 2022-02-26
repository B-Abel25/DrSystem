
using DoctorSystem.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace DoctorSystem.Dtos
{
    public class RegisterDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string BirthDate { get; set; }
        [Required]
        public string MedNumber { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        [StringLength(16,MinimumLength = 8)]
        public string Password { get; set; }
        [Required]
        public string DoctorSealNumber { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public int PostCode { get; set; }
        [Required]
        public string Street { get; set; }
        [Required]
        public string HouseNumber { get; set; }
    }
}
