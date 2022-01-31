using MySql.EntityFrameworkCore.DataAnnotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoctorSystem.Entities
{
    [Table("Cities")]
    [MySqlCharset("utf8")]
    public class City : AbstractIdentifiable
    {
        [MySqlCollation("utf8_hungarian_ci")]
        [Column("City", TypeName = "varchar")]
        [StringLength(22)]
        public string Name { get; set; }
        [ForeignKey("Id")]
        public string CountyId { get; set; }
        public County County { get; set; }
    }
}
