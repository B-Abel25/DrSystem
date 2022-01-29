using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MySql.EntityFrameworkCore.DataAnnotations;

namespace DoctorSystem.Entities
{
    [Table("Counties")]
    [MySqlCharset("utf8")]
    public class County : AbstractIdentifiable
    {
        [MySqlCollation("utf8_hungarian_ci")]
        [Column("County",TypeName = "varchar")]
        [StringLength(22)]
        public string Name { get; set; }
    }
}