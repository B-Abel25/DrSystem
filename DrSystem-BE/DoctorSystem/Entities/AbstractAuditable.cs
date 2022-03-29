using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DoctorSystem
{
    public class AbstractAuditable : AbstractIdentifiable 
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;

        /*// TODO https://stackoverflow.com/questions/37285948/how-to-set-created-date-and-modified-date-to-enitites-in-db-first-approach/47980095
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime ModifyDate { get; set; } = DateTime.UtcNow;
 
        */
    }
}
