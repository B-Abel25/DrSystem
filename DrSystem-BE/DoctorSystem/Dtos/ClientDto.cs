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
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; } 
        public PlaceDto Place { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public string MedNumber { get; set; }
        public bool Member { get; set; }
        public DoctorDto Doctor { get; set; }

        public ClientDto()
        {

        }

        public ClientDto(Client c)
        {
            this.Id = c.Id;
            this.Name = c.Name;
            this.BirthDate = c.BirthDate;
            this.Email = c.Email;
            this.PhoneNumber = c.PhoneNumber;
            this.Place = new PlaceDto(c.Place);
            this.Street = c.Street;
            this.HouseNumber = c.HouseNumber;
            this.MedNumber = c.MedNumber;
            this.Member = c.Member;
            this.Doctor = new DoctorDto(c.Doctor);
        }

       
    }
}
