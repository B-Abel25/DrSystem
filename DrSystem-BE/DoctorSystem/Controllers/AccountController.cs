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
        private readonly IConfiguration _config;
        private readonly Random random;

        public AccountController(ILogger<AccountController> logger, BaseDbContext context, ITokenService tokenService, EmailService emailService, IConfiguration config)
        {
            _logger = logger;
            //_accountService = registerService;
            _tokenService = tokenService;
            _context = context;
            _emailService = emailService;
            _config = config;
        }

        [Route("client/register")]
        [HttpPost]
        public async Task<ActionResult> Register(RegisterDto registerDto)
        {
            var client = await _context._clients.SingleOrDefaultAsync(x => x.MedNumber == registerDto.MedNumber);
            if (client != null) return BadRequest("Helytelen TAJ szám");

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
        public async Task<ActionResult<ClientDto>> ClientLogin(ClientLoginDto loginDto)
        {
            var client = await _context._clients.SingleOrDefaultAsync(x => x.MedNumber == loginDto.MedNumber);


            if (client == null) return Unauthorized("Regisztrálatlan TAJ szám");
            else if (!(client.EmailToken.Length == 37 || client.EmailToken == "true")) return Unauthorized("Hitelesítetlen E-mail cím");
            else if (!client.Member) return Unauthorized("A benyújtott kérelmet még nem fogadta el az orvos");
            var hmac = new HMACSHA512(client.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != client.Password[i])
                {
                    return Unauthorized("Helytelen jelszó");
                }
            }

            return new ClientDto()
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
                if (lostDto.UserNumber.Length == 9)
                {


                    //TODO csak akkor kérhet új jelszót ha hitelesítve van az emailje illetve a doki elfogadta?
                    var client = await _context._clients.SingleOrDefaultAsync(x => x.MedNumber == lostDto.UserNumber);
                    if (client == null) return Unauthorized("MedNumber does not exist");

                    client.EmailToken = Guid.NewGuid().ToString() + (char) random.Next(97,123) ;
                    await _context.SaveChangesAsync();

                    _emailService.NewPassword(client);
                    return Accepted();
                }
                if (lostDto.UserNumber.Length == 5)
                {
                    var doctor = await _context._doctors.SingleOrDefaultAsync(x => x.SealNumber == lostDto.UserNumber);
                    if (doctor == null) return Unauthorized("MedNumber does not exist");

                    doctor.EmailToken = Guid.NewGuid().ToString();
                    await _context.SaveChangesAsync();

                    _emailService.NewPassword(doctor);
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
                var user = await _context._clients.SingleOrDefaultAsync(x => x.EmailToken == newDto.EmailToken);
                if (user == null)
                {
                    user = await _context._clients.SingleOrDefaultAsync(x => x.EmailToken == newDto.EmailToken);
                    if (user == null)
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


        //TODO csak gettel működik DE MIÉRT?????
        [HttpGet]
        [Route("validate-email/{token}")]
        public async Task<ActionResult> ValidateEmail(string token)
        {
            var client = await _context._clients.SingleOrDefaultAsync(x => x.EmailToken == token.ToString());
            if (client == null) return Unauthorized("Helytelen azonosító");
            client.EmailToken = "true";
            await _context.SaveChangesAsync();
            return Redirect(_config["Root"] +"/login");
        }


        [HttpPut]
        [Route("doctor/login")]
        public async Task<ActionResult<DoctorDto>> DoctorLogin(DoctorLoginDto loginDto)
        {
            var doc = await _context._doctors.SingleOrDefaultAsync(x => loginDto.SealNumber == x.SealNumber);

            if (doc == null) return Unauthorized("Helytelen TAJ szám");
            else if (!(doc.EmailToken.Length == 10 || doc.EmailToken == "true")) return Unauthorized("Hitelesítetlen E-mail cím");
          
            var hmac = new HMACSHA512(doc.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != doc.Password[i])
                {
                    return Unauthorized("Helytelen jelszó");
                }
            }

            return new DoctorDto()
            {
                MedNumber = doc.SealNumber,
                Token = _tokenService.CreateToken(doc)
            };
        }


        [Route("client/delete/{token}")]
        [HttpGet] //Ez itt igazábol DELETE csak csaltam
        public async Task<ActionResult> DeleteClient(string token)
        {
            var client = await _context._clients.SingleOrDefaultAsync(x => x.EmailToken == token.ToString());
            if (client == null || token == "true") return Unauthorized("Helytelen azonosító");
            _context._clients.Remove(client);
            await _context.SaveChangesAsync();
            return Redirect(_config["Root"] + "/login");
        }

       
    }
}
