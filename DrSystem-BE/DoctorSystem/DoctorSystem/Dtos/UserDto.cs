using DoctorSystem.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoctorSystem.Dtos
{
    public class UserDto
    {
        public UserDto()
        { 
        
        }

        public UserDto(User user)
        {
            this.UserId = user.Id;
            this.Name = user.Name;
            this.MedNumber = user.MedNumber;
            this.Email = user.Email;
            this.Password = user.Password;
            this.Member = user.Member;
            this.PhoneNumber = user.PhoneNumber;
        }

        public string UserId { get; set; }
        public string Name { get; set; }
        public string MedNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public bool Member { get; set; }
    }
}
