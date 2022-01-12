using DoctorSystem.Dtos;
using DoctorSystem.Entities;
using DoctorSystem.Entities.Contexts;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoctorSystem.Services
{
    public interface IUserService
    {
        UserDto PostUser(UserDto userDto);
        UserDto PutUser(string Id, UserDto userDto);
        UserDto DeleteUser(string userDtoId);
        UserDto GetUser(string userDtoId);
    }


    public class UserService : IUserService
    {

        private readonly ILogger<UserService> _logger;
        private readonly UserDbContext _dbContext;
        public UserService()
        {
        }

        public UserService(ILogger<UserService> logger, UserDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }



        public UserDto PostUser(UserDto userDto)
        {
            _dbContext._users.Add(new User(userDto));      
            _dbContext.SaveChanges();
            return userDto;
        }

        public UserDto PutUser(string id, UserDto userDto)
        {
            User userEntity = _dbContext._users.Find(id);
            userDto.UserId = userEntity.Id;
            userEntity.Name = userDto.Name;
            userEntity.Email = userDto.Email;
            userEntity.Password = userDto.Password;



            _dbContext._users.Update(userEntity);
            _dbContext.SaveChanges();
            return userDto;

        }

        public UserDto DeleteUser(string userDtoId)
        {

            User userEntity = _dbContext._users.Find(userDtoId);
            _dbContext.Remove(userEntity);
            _dbContext.SaveChanges();
            return new UserDto(userEntity);

            /*UserDto del = new UserDto(_dbContext.getUserById(userDtoId));
            _dbContext._user.Remove(new User(del));
            _dbContext.SaveChanges();
            return del;*/
        }

        public UserDto GetUser(string userDtoId)
        {
            return new UserDto(_dbContext._users.Find(userDtoId));
        }

        private void ExchangeData(User userOld, UserDto userNew)
        {
           
            userNew.UserId = userOld.Id;
            //userNew.CreateDate = userOld.CreateDate;

        }



    }
}
