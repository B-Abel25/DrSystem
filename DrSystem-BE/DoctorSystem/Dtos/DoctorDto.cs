namespace DoctorSystem.Dtos
{
    public class DoctorDto
    {
        [Required]
        public string MedNumber { get; set; }
        [Required]
        public string Token { get; set; }
    }
}
