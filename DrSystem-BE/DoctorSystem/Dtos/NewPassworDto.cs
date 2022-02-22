using System.ComponentModel.DataAnnotations;

namespace DoctorSystem.Dtos
{
    public class NewPassworDto
    {
        [Required]
        public string EmailToken { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
