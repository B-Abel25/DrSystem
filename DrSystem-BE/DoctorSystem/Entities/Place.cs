using MySql.EntityFrameworkCore.DataAnnotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoctorSystem.Entities
{
    [Table("Cities")]
    [MySqlCharset("utf8")]
    public class Place : AbstractIdentifiable
    {
        
        public int PostCode { get; set; }
        [ForeignKey("Id")]
        public string CityId { get; set; }
        public City City { get; set; }

    }
}
