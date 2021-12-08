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
            this.Id = user.Id;
            this.FirstName = user.FirstName;
            this.LastName = user.LastName;
            this.MedNumber = user.MedNumber;
            this.Email = user.Email;
            this.Password = user.Password;
            this.Member = user.Member;
            //this.Active = user.Active;
        }

        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MedNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Member { get; set; }
        //public bool Active { get; set; }
    }
}
