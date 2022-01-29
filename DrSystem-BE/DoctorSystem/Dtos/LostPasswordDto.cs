using System.ComponentModel.DataAnnotations;

namespace DoctorSystem.Dtos
{
    public class LostPasswordDto
    {
        [Required]
        public string MedNumber { get; set; }
    }
}
