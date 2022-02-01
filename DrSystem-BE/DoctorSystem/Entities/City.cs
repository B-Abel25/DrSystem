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
        [MySqlCollation("utf8_hungarian_ci")]
        [Column("Name", TypeName = "varchar")]
        [StringLength(22)]
        public string Name { get; set; }

        [JsonIgnore]
        public ICollection<Place> Places { get; set; }
        /*
        [ForeignKey("Id")]
        public string CountyId { get; set; }
        */
        public County County { get; set; }
        public string CountyId { get; set; }
    }
}
