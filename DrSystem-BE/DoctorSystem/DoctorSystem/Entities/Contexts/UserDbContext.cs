using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoctorSystem.Entities.Contexts
{
    public class UserDbContext : AbstractDbContext<User>
    {
        public UserDbContext(IConfiguration configuration) : base(configuration)
        {
        }
        public UserDbContext() { }
        public override void addToEntites(User entity)
        {
            base._user.Add(entity);
        }
        public override void removeFromEntites(User entity)
        {
            base._user.Remove(entity);
        }
        public override void updateEntity(User entity)
        {
            base._user.Update(entity);
        }

        public virtual List<User> getAllUsersByIds(List<string> ids)
        {
            var rooms = base._user.AsQueryable()
                     .Where(room => ids.Contains(room.Id)) // server-evaluated
                     .ToList();
            return rooms;
        }

        public virtual List<User> getAllUsersByMedNumber(List<string> MedNumbers)
        {
            var rooms = base._user.AsQueryable()
                     .Where(room => MedNumbers.Contains(room.MedNumber)) // server-evaluated
                     .ToList();
            return rooms;
        }

        public virtual User getUserById(string id)
        {
            var room = base._user.AsQueryable().Where(room => room.Id == id);
            return room.First();
        }

        public virtual User deleteUserById(string id)
        {
            return new User();
        }


    }
}
