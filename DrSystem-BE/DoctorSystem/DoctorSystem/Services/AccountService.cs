using DoctorSystem.Dtos;
using DoctorSystem.Entities;
using DoctorSystem.Entities.Contexts;
using DoctorSystem.Model.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DoctorSystem.Services
{
    public class AccountService
    {
        private readonly ILogger<AccountService> _logger;
        private readonly BaseDbContext _customDbContext;
        private readonly EmailService _emailService;


        public AccountService(ILogger<AccountService> logger, BaseDbContext customDbContext, EmailService emailService)
        {
            _logger = logger;
            _customDbContext = customDbContext;
            _emailService = emailService;
        }

        public void register(RegisterDto registerDto)
        {
            //Hiba kezelés
            if (this.getUserByMedNumber(registerDto.MedNumber).Result != null)
            {
                throw new AlreadyRegisteredException("MedNumber already registered");
            };
            User user = new User(registerDto);
            this._customDbContext._users.Add(user);
            this._customDbContext.SaveChanges();

            string content = $"<h1>Ezt az emailt azért kapja mert regisztrált a DrSystem-be kedves "+user.Name+"</h1>"+
                "<p>Amint az Doktor úr/Doktornő elfogadja vagy elutasítja jelentkezési kérelmét azonnal értesítjük emailbe.</p>";
            this._emailService.sendEmail(registerDto.Email, content, "Regisztráció visszajelzése");
        }


        internal UserDto getUser(string id)
        {
            return new UserDto(getHandledUser(getById(id)));
        }

        internal void login(LoginDto loginDto)
        {
            throw new NotImplementedException();
        }

        internal UserDto modifyUser(string id, UserDto userDto)
        {
            var user = getHandledUser(getById(id));
            user.Name = userDto.Name;
            user.Email = userDto.Email;
            user.PhoneNumber = userDto.PhoneNumber;
            user.Password = userDto.Password;
            this._customDbContext._users.Update(user);
            this._customDbContext.SaveChanges();
            return userDto;
        }

        internal UserDto deleteUser(string id)
        {
            var user = getHandledUser(getById(id));
            UserDto userDto = new UserDto(user);
            // TODO email küldés confirmation-ről?
            this._customDbContext._users.Remove(user);
            this._customDbContext.SaveChanges(); //TODO ha már a validáció elkészült
            return userDto;

        }

        internal LostPasswordDto lostPassword(LostPasswordDto lostPasswordDto)
        {
            var user = getHandledUser(getUserByMedNumber(lostPasswordDto.MedNumber));

            user.Password = GenerateTemporaryPassword(8);
            string content = "<h1>Jelszava megváltozott</h1>" +
                "<h3>Az új jelszava: <strong>"+user.Password+"</strong></h3>";

            this._emailService.sendEmail(user.Email, content, "New Password Created");
            this._customDbContext.SaveChanges();
            return lostPasswordDto;
        }

        /*
        internal void newPassword(NewPasswordDto newPasswordDto)
        {
            var user = getHandledUser(getUserByUsernameAndToken(newPasswordDto.Email, newPasswordDto.Token));
            user.Password = newPasswordDto.Password;
            this._customDbContext.SaveChanges();

        }
        */

        private async Task<User> getUserByMedNumberAndPassword(string medNumber, string password)
        {
            return await this._customDbContext._users.FirstOrDefaultAsync(u => u.MedNumber == medNumber && u.Password == password);
        }
        private async Task<User> getById(string id)
        {
            return await this._customDbContext._users.FirstOrDefaultAsync(u => u.Id == id);
        }
        private Task<User> getUserByMedNumber(string medNumber)
        {
            return this._customDbContext._users.FirstOrDefaultAsync(u => u.MedNumber == medNumber);
        }
        private User getHandledUser(Task<User> userOpt)
        {
            return getMemberedHandled(getMissingHandled(userOpt));
        }
        //TODO  Ez itt mi és minek????
        private User getMissingHandled(Task<User> userOpt)
        {
            if (userOpt.Result == null)
            {
                throw new BONotFoundException("Business Object not Found");
            }
            return userOpt.Result;
        }

        private User getMemberedHandled(User user)
        {
            if (!user.Member)
            {
                throw new NotMemberException("User account not accepted by the Doc!");
            }
            return user;
        }
        string GenerateTemporaryPassword(int length)
        {
            Random random = new Random();
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789#&@";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
}
}
