using DoctorSystem.Entities;
using DoctorSystem.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DoctorSystem.Dtos
{
    public class ClientDto : UserDto
    {
        public string MedNumber { get; set; }
        public bool Member { get; set; }
        public DoctorDto Doctor { get; set; }
        public string MotherName { get; set; }
        public string BirthPlace { get; set; }

        public ClientDto()
        {

        }

        public ClientDto(Client c, string token)
        {
            this.Name = c.Name;
            this.BirthDate = c.BirthDate.ToString("yyyy.MM.dd");
            this.Email = c.Email;
            this.PhoneNumber = c.PhoneNumber;
            this.Place = new PlaceDto(c.Place);
            this.Street = c.Street;
            this.HouseNumber = c.HouseNumber;
            this.MedNumber = c.MedNumber;
            this.Member = c.Member;
            this.Doctor = new DoctorDto(c.Doctor);
            this.Token = token;
            this.MotherName = c.MotherName;
            this.BirthPlace = c.BirthPlace.Name;
        }

        public ClientDto(Client c)
        {
            this.Name = c.Name;
            this.BirthDate = c.BirthDate.ToString("yyyy.MM.dd");
            this.Email = c.Email;
            this.PhoneNumber = c.PhoneNumber;
            this.Place = new PlaceDto(c.Place);
            this.Street = c.Street;
            this.HouseNumber = c.HouseNumber;
            this.MedNumber = c.MedNumber;
            this.Member = c.Member;
            this.Doctor = new DoctorDto(c.Doctor);
            this.MotherName = c.MotherName;
            this.BirthPlace = c.BirthPlace.Name;
        }
    }
}
