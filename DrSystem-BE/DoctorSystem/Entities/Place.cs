using MySql.EntityFrameworkCore.DataAnnotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoctorSystem.Entities
{
    [Table("Place")]
    [MySqlCharset("utf8")]
    public class Place : AbstractIdentifiable
    {
        [Required]
        public int PostCode { get; set; }
        [Column("CityId", TypeName = "varchar(37)")]
        [Required]
        public City City { get; set; }
    }
}
