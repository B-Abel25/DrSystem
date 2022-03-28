using System.ComponentModel.DataAnnotations;

namespace DoctorSystem.Controllers
{
    public class DeleteMessageDto
    {
        [Required]
        public string DateSent { get; set; }
        [Required]
        public string Content { get; set; }
        
    }
}