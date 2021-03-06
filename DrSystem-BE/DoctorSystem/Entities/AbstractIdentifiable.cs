using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DoctorSystem
{
    public abstract class AbstractIdentifiable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Column("Id", TypeName = "varchar(37)")]
        public string Id { get; set; }

        
        //public bool Active { get; set; }
    }
}
