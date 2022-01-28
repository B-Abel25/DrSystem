using System.ComponentModel.DataAnnotations;

namespace DoctorSystem.Dtos
{
    public class NewPassworDto
    {
        [Required]
        public string Token { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
