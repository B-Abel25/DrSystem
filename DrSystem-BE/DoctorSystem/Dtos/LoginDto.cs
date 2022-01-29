using System.ComponentModel.DataAnnotations;

namespace DoctorSystem.Dtos
{
    public class LoginDto
    {
        [Required]
        public string MedNumber { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
