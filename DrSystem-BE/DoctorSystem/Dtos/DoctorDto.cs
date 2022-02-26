using DoctorSystem.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DoctorSystem.Dtos
{
    public class DoctorDto : UserDto
    {
        public DoctorDto(Doctor doc, string token)
        {
            this.Name = doc.Name;
            this.BirthDate = doc.BirthDate.ToString("{yyyy.MM.dd}");
            this.Email = doc.Email;
            this.PhoneNumber = doc.PhoneNumber;
            this.Place = new PlaceDto(doc.Place);
            this.Street = doc.Street;
            this.HouseNumber = doc.HouseNumber;
            this.SealNumber = doc.SealNumber;
            this.Token = token;
        }

        public DoctorDto(Doctor doc)
        {
            this.Name = doc.Name;
            this.BirthDate = doc.BirthDate.ToString("{yyyy.MM.dd}");
            this.Email = doc.Email;
            this.PhoneNumber = doc.PhoneNumber;
            this.Place = new PlaceDto(doc.Place);
            this.Street = doc.Street;
            this.HouseNumber = doc.HouseNumber;
            this.SealNumber = doc.SealNumber;
        }
        public DoctorDto()
        {
        }


        public ICollection<ClientDto> Clients { get; set; }
        public string SealNumber { get; set; }
    }
}
