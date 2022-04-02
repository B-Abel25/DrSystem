using MySql.EntityFrameworkCore.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoctorSystem.Entities
{
    [Table("Message")]
    [MySqlCharset("utf8")]
    public class Message : AbstractIdentifiable
    {
        [Column("Sender", TypeName = "varchar(37)")]
        [Required]
        public User Sender { get; set; }
        [Column("Sender", TypeName = "varchar(37)")]
        [Required]
        public User Reciever { get; set; }
        public DateTime? DateRead { get; set; }
        [Required]
        public DateTime DateSent { get; set; } = DateTime.Now;
        [MySqlCollation("utf8_hungarian_ci")]
        [Required]
        public string Content { get; set; }
    }
}
