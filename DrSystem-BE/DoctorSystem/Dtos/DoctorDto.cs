using DoctorSystem.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DoctorSystem.Dtos
{
    public class DoctorDto
    {
        public DoctorDto(Doctor doc)
        {
            this.Id = doc.Id;
            this.Name = doc.Name;
            this.BirthDate = doc.BirthDate.ToShortDateString();
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


        public string Id { get; set; }
        public string Name { get; set; }
        public string BirthDate { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public PlaceDto Place { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public ICollection<ClientDto> Clients { get; set; }
        public string SealNumber { get; set; }
    }
}
