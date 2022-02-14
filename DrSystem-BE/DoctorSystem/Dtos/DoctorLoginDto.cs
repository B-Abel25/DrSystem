using System.ComponentModel.DataAnnotations;

namespace DoctorSystem.Dtos
{
    public class DoctorLoginDto
    {
        [Required]
        public string SealNumber { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
