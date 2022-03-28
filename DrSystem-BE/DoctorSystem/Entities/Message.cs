using MySql.EntityFrameworkCore.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoctorSystem.Entities
{
    [Table("Message")]
    [MySqlCharset("utf8")]
    public class Message : AbstractIdentifiable
    {
        public User Sender { get; set; }
        public User Reciever { get; set; }
        public DateTime? DateRead { get; set; }
        public DateTime DateSent { get; set; } = DateTime.Now;
        [MySqlCollation("utf8_hungarian_ci")]
        public string Content { get; set; }
        public bool SenderDeleted { get; set; }
        public bool RecieverDeleted { get; set; }
    }
}
