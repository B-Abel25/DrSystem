using DoctorSystem.Entities;
using System;
using System.Collections.Generic;

namespace DoctorSystem.Dtos
{
    public class UserDto
    {
        public string Name { get; set; }
        public string BirthDate { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public PlaceDto Place { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public string Token { get; set; }
        public ICollection<MessageDto> MessagesRecieved { get; set; }
        public ICollection<MessageDto> MessagesSent { get; set; }

        
        public UserDto(User u)
        {
            this.Name = u.Name;
            this.BirthDate = u.BirthDate.ToString("yyyy.MM.dd");
            this.Email = u.Email;
            this.PhoneNumber = u.PhoneNumber;
            this.Place = new PlaceDto(u.Place);
            this.Street = u.Street;
            this.HouseNumber = u.HouseNumber;
        }
        
        public UserDto()
        {

        }
    }
}
