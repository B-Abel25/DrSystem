using MySql.EntityFrameworkCore.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DoctorSystem.Entities
{
    [Table("City")]
    [MySqlCharset("utf8")]
    public class City : AbstractIdentifiable
    {
        [Required]
        [MySqlCollation("utf8_hungarian_ci")]
        [Column("Name", TypeName = "varchar(20)")]
        public string Name { get; set; }
        public ICollection<Place> Places { get; set; }
        [Required]
        [Column("CountyId", TypeName = "varchar(37)")]
        public County County { get; set; }
    }
}
