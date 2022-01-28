using DoctorSystem.Model.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoctorSystem.Model
{
    public class Place
    {
        [Required]
        [Key]
        public int PostCode { get; set; }

        [Column(TypeName = "nvarchar(24)")]
        public City City { get; set; }

        [Column(TypeName = "nvarchar(24)")]
        public County County { get; set; }
    }
}
