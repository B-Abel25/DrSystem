﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using MySql.EntityFrameworkCore.DataAnnotations;

namespace DoctorSystem.Entities
{
    [Table("County")]
    [MySqlCharset("utf8")]
    public class County : AbstractIdentifiable
    {
        [MySqlCollation("utf8_hungarian_ci")]
        [Column("Name",TypeName = "varchar(22)")]
        [Required]
        public string Name { get; set; }
        public ICollection<City> Cities { get; set; }
    }
}