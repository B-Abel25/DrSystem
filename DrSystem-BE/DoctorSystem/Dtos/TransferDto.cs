using System.ComponentModel.DataAnnotations;

namespace DoctorSystem.Dtos
{
    public class TransferDto
    {
        [Required]
        public string MedNumber { get; set; }
        [Required]
        public string NewDoctorSealNumber { get; set; }
    }
}
