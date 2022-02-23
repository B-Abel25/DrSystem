using DoctorSystem.Dtos;
using DoctorSystem.Entities;
using DoctorSystem.Entities.Contexts;
using DoctorSystem.Interfaces;
using DoctorSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

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
        private readonly RouterService _router;
        private readonly Random random = new Random();

        public AccountController(ILogger<AccountController> logger, BaseDbContext context, ITokenService tokenService, EmailService emailService, RouterService routerService)
        {
            _logger = logger;
            _tokenService = tokenService;
            _context = context;
            _emailService = emailService;
            _router = routerService;
        }

        [Route("client/register")]
        [HttpPost]
        public async Task<ActionResult> Register(RegisterDto registerDto)
        {
            var client = await _context._clients.SingleOrDefaultAsync(x => x.MedNumber == registerDto.MedNumber);
            if (UserExistsAsync(client))
            {
                return BadRequest("Ez a TAJ szám már létezik");
            }

            client = new Client();
            client.Name = registerDto.Name;
            client.MedNumber = registerDto.MedNumber;
            client.Email = registerDto.Email;
            client.PhoneNumber = registerDto.PhoneNumber;
            var hmac = new HMACSHA512();
            client.Password = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
            client.PasswordSalt = hmac.Key;
            client.EmailToken = Guid.NewGuid().ToString();
            client.Place = await _context._place.SingleOrDefaultAsync(x => x.Id == registerDto.PlaceId);
            client.Street = registerDto.Street;
            client.HouseNumber = registerDto.HouseNumber;
            client.BirthDate = DateTime.Parse(registerDto.BirthDate);
            client.Doctor = await _context._doctors.SingleOrDefaultAsync(x => x.Id == registerDto.DoctorId);
            client.Member = client.Doctor.Place == client.Place ? true : false;

            _context._clients.Add(client);

            this._emailService.SuccessfulRegistration(client);

            await _context.SaveChangesAsync();

            return Accepted();
        }

        [Route("client/login")]
        [HttpPost]
        public async Task<ActionResult<object>> ClientLogin(ClientLoginDto loginDto)
        {
            var client = await _context._clients.SingleOrDefaultAsync(x => x.MedNumber == loginDto.MedNumber);


            if (!UserExistsAsync(client))
            {
                return Unauthorized("Regisztrálatlan TAJ szám");
            }

            else if (!(client.EmailToken.Length == 37 || client.EmailToken == "true"))
            {
                return Unauthorized("Hitelesítetlen E-mail cím");
            }
            else if (!client.Member)
            {
                return Unauthorized("A benyújtott kérelmet még nem fogadta el az orvos");
            }
            var hmac = new HMACSHA512(client.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != client.Password[i])
                {
                    return Unauthorized("Helytelen jelszó");
                }
            }

            return new { Id = client.Id, Token = _tokenService.CreateToken(client) };
        }

        [Route("lost-password")]
        [HttpPut]
        public async Task<ActionResult> LostPassword(LostPasswordDto lostDto)
        {
            try
            {
                User user;
                if (lostDto.UserNumber.Length == 9)
                {
                    user = await _context._clients.SingleOrDefaultAsync(x => x.MedNumber == lostDto.UserNumber);
                    if (!UserExistsAsync(user))
                    {
                        return Unauthorized("Nem létező TAJ szám");
                    }
                    user.EmailToken = Guid.NewGuid().ToString() + (char)random.Next(97, 123);
                    _emailService.NewPassword(user);
                    await _context.SaveChangesAsync();
                    return Accepted();
                }
                else if (lostDto.UserNumber.Length == 5)
                {
                    user = await _context._doctors.SingleOrDefaultAsync(x => x.SealNumber == lostDto.UserNumber);
                    if (!UserExistsAsync(user))
                    {
                        return Unauthorized("Nem létező pecsétszám");
                    }
                    user.EmailToken = Guid.NewGuid().ToString() + (char)random.Next(97, 123);
                    _emailService.NewPassword(user);
                    await _context.SaveChangesAsync();
                    return Accepted();
                }
                   
                return Unauthorized("Érvénytelen azonosító szám");
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
                User user = await _context._clients.SingleOrDefaultAsync(x => x.EmailToken == newDto.EmailToken);
                if (!UserExistsAsync(user))
                {
                    user = await _context._doctors.SingleOrDefaultAsync(x => x.EmailToken == newDto.EmailToken);
                    if (!UserExistsAsync(user))
                    {
                        return Unauthorized("Helytelen azonosító");
                    }
                }
                user.EmailToken = "true";
                var hmac = new HMACSHA512();
                user.Password = hmac.ComputeHash(Encoding.UTF8.GetBytes(newDto.Password));
                user.PasswordSalt = hmac.Key;
                await _context.SaveChangesAsync();
                return Accepted();
            }
            catch ( Exception e)
            {
                return Unauthorized(e.Message);
            }
        }


        [HttpGet]
        [Route("validate-email/{token}")]
        public async Task<ActionResult> ValidateEmail(string token)
        {
            var client = await _context._clients.SingleOrDefaultAsync(x => x.EmailToken == token.ToString());
            if (!UserExistsAsync(client))
            {
                return Unauthorized("Helytelen azonosító");
            }
            client.EmailToken = "true";
            await _context.SaveChangesAsync();
            return Redirect(_router.Route("/login"));
        }


        [HttpPut]
        [Route("doctor/login")]
        public async Task<ActionResult<object>> DoctorLogin(DoctorLoginDto loginDto)
        {
            var doc = await _context._doctors.Include(x => x.Place.City.County).SingleOrDefaultAsync(x => loginDto.SealNumber == x.SealNumber);
            if (doc == null)
            {
                return Unauthorized("Helytelen pecsétszám szám");
            }
            else if (!(doc.EmailToken.Length == 37 || doc.EmailToken == "true"))
            {
                return Unauthorized("Hitelesítetlen E-mail cím");
            }
            var hmac = new HMACSHA512(doc.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != doc.Password[i])
                {
                    return Unauthorized("Helytelen jelszó");
                }
            }
            
            return new { Id = doc.Id, Token = _tokenService.CreateToken(doc) };
        }


        [Route("client/delete")]
        [HttpGet] //Ez itt igazábol DELETE csak csaltam
        public async Task<ActionResult> DeleteClient(string token)
        {
            var client = await _context._clients.SingleOrDefaultAsync(x => x.EmailToken == token.ToString());
            if (client == null || token == "true")
            {
                return Unauthorized("Helytelen azonosító");
            }
            _context._clients.Remove(client);
            await _context.SaveChangesAsync();
            return Redirect(_router.Route("/login"));
        }

       private bool UserExistsAsync(User u)
       {
            if (u == null)
            {
                return false;
            }
            return true;
       }
    }
}
