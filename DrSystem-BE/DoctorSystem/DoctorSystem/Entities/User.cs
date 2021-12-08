using DoctorSystem.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoctorSystem.Entities
{
    public class User : AbstractAuditable
    {
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string MedNumber { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public bool Member { get; set; }

        public User()
        {

        }

        public User(UserDto dto)
        {
            this.Id = dto.Id;
            this.FirstName = dto.FirstName;
            this.LastName = dto.LastName;
            this.MedNumber = dto.MedNumber;
            this.Email = dto.Email;
            this.Password = dto.Password;
            this.Member = dto.Member;
            //this.Active = dto.Active;
        }

        public User(string id, string firstName, string lastName, string medNumber, string email, string password, bool member/*, bool active*/)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            MedNumber = medNumber;
            Email = email;
            Password = password;
            Member = member;
            //Active = active;
        }
    }
}
