using DoctorSystem.Dtos;
using DoctorSystem.Entities;
using DoctorSystem.Entities.Contexts;
using DoctorSystem.Interfaces;
using DoctorSystem.Model.Exceptions;
using DoctorSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

namespace DoctorSystem.Controllers
{
    [ApiController]
    [Route("public")]
    public class AccountController : ControllerBase
    {


        private readonly ILogger<AccountController> _logger;
        private readonly BaseDbContext _context;
        private readonly ITokenService _tokenService;
        private readonly EmailService _emailService;  

        public AccountController(ILogger<AccountController> logger, BaseDbContext context, ITokenService tokenService, EmailService emailService)
        {
            _logger = logger;
            //_accountService = registerService;
            _tokenService = tokenService;
            _context = context;
            _emailService = emailService;
        }

        [Route("register")]
        [HttpPost]
        public async Task<ActionResult> Register(RegisterDto registerDto)
        {
            if (await UserExists(registerDto.MedNumber)) return BadRequest("MedNumber already exists");

            User user = new User();

            user.Name = registerDto.Name;
            user.MedNumber = registerDto.MedNumber;
            user.Email = registerDto.Email;
            user.PhoneNumber = registerDto.PhoneNumber;
            var hmac = new HMACSHA512();
            user.Password = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
            user.PasswordSalt = hmac.Key;
            user.Token = GenerateToken(8);
            user.Place = await _context._place.SingleOrDefaultAsync(x => x.Id == registerDto.PlaceId);
            user.Street = registerDto.Street;
            user.HouseNumber = registerDto.HouseNumber;
            user.BirthDate = DateTime.Parse(registerDto.BirthDate);
            user.Doctor = await _context._doctors.SingleOrDefaultAsync(x => x.Id == registerDto.DoctorId);
            user.Member = user.Doctor.Place == user.Place ? true : false;

            _context._users.Add(user);

            await _context.SaveChangesAsync();

            string link = "https://localhost:44347/validate-email/" + user.Token;
            string content = $"<h1>Kérjük hitelesítse email címét!</h1>" +
                "<p>Kérjük nyomjon az alábbi gombra és hitelesítse email címét</p>" +
                 "<button style = \"background: #1A1A1A; Color: #FFF;  padding: 14px 25px;\"" +
                    "onclick = \"window.open(\"" + link + "\") > Emailcím hitelesítése </button>";

            this._emailService.sendEmail(registerDto.Email, content, "Regisztráció visszajelzés");

            return Accepted();
        }

        [Route("login")]
        [HttpPost]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _context._users.SingleOrDefaultAsync(x => x.MedNumber == loginDto.MedNumber);

            if (user == null) return Unauthorized("Invalid MedNumber");
            else if (!(user.Token.Length == 10 || user.Token == "true")) return Unauthorized("Your email is not verifyed");
            else if (!user.Member) return Unauthorized("The requested Doctor still did not accepted");
            var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.Password[i])
                {
                    return Unauthorized("Invalid Password");
                }
            }

            return new UserDto()
            {
                MedNumber = user.MedNumber,
                Token = _tokenService.CreateToken(user)
            };
        }

        [Route("lost-password")]
        [HttpPut]
        public async Task<ActionResult> LostPassword(LostPasswordDto lostDto)
        {
            try
            {
                //TODO csak akkor kérhet új jelszót ha hitelesítve van az emailje illetve a doki elfogadta?
                var user = await _context._users.SingleOrDefaultAsync(x => x.MedNumber == lostDto.MedNumber);
                if (user == null) return Unauthorized("MedNumber does not exist");

                user.Token = GenerateToken(10);
                await _context.SaveChangesAsync();
                string link = "https://localhost:44347/lost-password-verify/" + user.Token;
                string content = $"<h1>Új jelszó</h1>" +
                    "<p>Kérjük nyomjon az alábbi gombra majd írja be új jelszavát!</p>" +
                    "<button style = \"background: #1A1A1A; Color: #FFF;  padding: 14px 25px;\"" +
                    "onclick = \"window.open(\"" + link + "\") > Új jelszót kérek </button>";

                    _emailService.sendEmail(user.Email,content,"Új jelszó");
                return Accepted();
            }
            catch (Exception e)
            {
                return Unauthorized(e.Message);
            }
        }

        [Route("new-password")]
        [HttpPost]
        public async Task<ActionResult> NewPassword(NewPassworDto newDto)
        {
            try
            {
                var user = await _context._users.SingleOrDefaultAsync(x => x.Token == newDto.Token);
                if (user == null) return Unauthorized("Invalid token");
                user.Token = "true";
                var hmac = new HMACSHA512();
                user.Password = hmac.ComputeHash(Encoding.UTF8.GetBytes(newDto.Password));
                user.PasswordSalt = hmac.Key;
                return Accepted();
            }
            catch ( Exception e)
            {
                return Unauthorized(e.Message);
            }
        }


        private async Task<bool> UserExists(string medNumber)
        {
            return await _context._users.AnyAsync(x => x.MedNumber == medNumber);
        }

        private string GenerateToken(int length)
        {
            Random random = new Random();
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789#&@";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
