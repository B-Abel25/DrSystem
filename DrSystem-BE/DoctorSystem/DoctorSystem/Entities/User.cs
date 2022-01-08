using DoctorSystem.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoctorSystem.Entities
{
    public class User : AbstractAuditable
    {
    public string Name { get; set; }
    public string MedNumber { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string PhoneNumber { get; set; }
    public bool Member { get; set; }

        public User()
        {

        }

        public User(UserDto dto)
        {
            this.Id = dto.UserId;
            this.Name = dto.Name;
            this.MedNumber = dto.MedNumber;
            this.Email = dto.Email;
            this.Password = dto.Password;
            this.Member = dto.Member;
            this.PhoneNumber = PhoneNumber;
        }

        public User(RegisterDto dto)
        {
            
            this.Name = dto.Name;
            this.MedNumber = dto.MedNumber;
            this.Email = dto.Email;
            this.Password = dto.Password;
            this.Member = false;
            this.PhoneNumber = dto.PhoneNumber;
        }
    }
}
