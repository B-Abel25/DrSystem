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

        [Route("client/register")]
        [HttpPost]
        public async Task<ActionResult> Register(RegisterDto registerDto)
        {
            if (await ClientExists(registerDto.MedNumber)) return BadRequest("MedNumber already exists");

            Client client = new Client();

            client.Name = registerDto.Name;
            client.MedNumber = registerDto.MedNumber;
            client.Email = registerDto.Email;
            client.PhoneNumber = registerDto.PhoneNumber;
            var hmac = new HMACSHA512();
            client.Password = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
            client.PasswordSalt = hmac.Key;
            client.Token = GenerateToken(8);
            client.Place = await _context._place.SingleOrDefaultAsync(x => x.Id == registerDto.PlaceId);
            client.Street = registerDto.Street;
            client.HouseNumber = registerDto.HouseNumber;
            client.BirthDate = DateTime.Parse(registerDto.BirthDate);
            client.Doctor = await _context._doctors.SingleOrDefaultAsync(x => x.Id == registerDto.DoctorId);
            client.Member = client.Doctor.Place == client.Place ? true : false;

            _context._clients.Add(client);

            this._emailService.SuccesfulRegistration(client);

            await _context.SaveChangesAsync();

            return Accepted();
        }

        [Route("client/login")]
        [HttpPost]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            _logger.Log("login");
            var client = await _context._clients.SingleOrDefaultAsync(x => x.MedNumber == loginDto.MedNumber);


            if (client == null) return Unauthorized("Invalid MedNumber");
            else if (!(client.Token.Length == 10 || client.Token == "true")) return Unauthorized("Your email is not verifyed");
            else if (!client.Member) return Unauthorized("The requested Doctor still did not accepted");
            var hmac = new HMACSHA512(client.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != client.Password[i])
                {
                    return Unauthorized("Invalid Password");
                }
            }

            return new UserDto()
            {
                MedNumber = client.MedNumber,
                Token = _tokenService.CreateToken(client)
            };
        }

        [Route("lost-password")]
        [HttpPut]
        public async Task<ActionResult> LostPassword(LostPasswordDto lostDto)
        {
            try
            {
                //TODO csak akkor kérhet új jelszót ha hitelesítve van az emailje illetve a doki elfogadta?
                var user = await _context._clients.SingleOrDefaultAsync(x => x.MedNumber == lostDto.MedNumber);
                if (user == null) return Unauthorized("MedNumber does not exist");

                user.Token = GenerateToken(10);
                await _context.SaveChangesAsync();
                string link = "https://localhost:44347/lost-password-verify/" + user.Token;
                string content = $"<h1>Új jelszó</h1>" +
                    "<p>Kérjük nyomjon az alábbi gombra majd írja be új jelszavát!</p>" +
                    "<button style = \"background: #1A1A1A; Color: #FFF;  padding: 14px 25px;\"" +
                    "onclick = \"window.open(\"" + link + "\") > Új jelszót kérek </button>";

                    //_emailService.sendEmail(user.Email,content,"Új jelszó");
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
                var client = await _context._clients.SingleOrDefaultAsync(x => x.Token == newDto.Token);
                if (client == null) return Unauthorized("Invalid token");
                client.Token = "true";
                var hmac = new HMACSHA512();
                client.Password = hmac.ComputeHash(Encoding.UTF8.GetBytes(newDto.Password));
                client.PasswordSalt = hmac.Key;
                return Accepted();
            }
            catch ( Exception e)
            {
                return Unauthorized(e.Message);
            }
        }


        //TODO csak gettel működik DE MIÉRT?????
        [HttpGet]
        [Route("validate-email/{token}")]
        public async Task<ActionResult> ValidateEmail(string token)
        {
            var client = await _context._clients.SingleOrDefaultAsync(x => x.Token == token.ToString());
            if (client == null) return Unauthorized("Invalid token");
            client.Token = "true";
            await _context.SaveChangesAsync();
            return Redirect($"https://localhost:4200/login");
        }

        private async Task<bool> ClientExists(string medNumber)
        {
            return await _context._clients.AnyAsync(x => x.MedNumber == medNumber);
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
