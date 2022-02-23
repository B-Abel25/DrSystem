using System;
using System.Collections.Generic;

namespace DoctorSystem.Dtos
{
    public abstract class UserDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string BirthDate { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailToken { get; set; }
        public PlaceDto Place { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public ICollection<MessageDto> MessagesRecieved { get; set; }
        public ICollection<MessageDto> MessagesSent { get; set; }
    }
}
