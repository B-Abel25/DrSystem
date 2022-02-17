using DoctorSystem.Entities;
using DoctorSystem.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DoctorSystem.Dtos
{
    public class ClientDto
    {
        [Required]
        public string MedNumber { get; set; }
        [Required]
        public string Token { get; set; }
    }
}
