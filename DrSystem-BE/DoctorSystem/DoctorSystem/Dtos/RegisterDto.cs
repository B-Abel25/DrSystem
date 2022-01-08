using System.ComponentModel.DataAnnotations;

namespace DoctorSystem.Dtos
{
    public class RegisterDto
    {
        public string Name { get; set; }
        public string MedNumber { get; set; }
        [Required]
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
    }
}
