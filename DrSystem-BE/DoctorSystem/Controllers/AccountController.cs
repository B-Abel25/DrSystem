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
       // private readonly BaseDbContext _context;
        private readonly IClientRepository _clientRepo;
        private readonly IDoctorRepository _doctorRepo;
        private readonly IPlaceRepository _placeRepo;
        private readonly ITokenService _tokenService;
        private readonly EmailService _emailService;
        private readonly RouterService _router;
        private readonly Random random = new Random();

        public AccountController(
            ILogger<AccountController> logger,
            BaseDbContext context,
            ITokenService tokenService,
            EmailService emailService,
            RouterService routerService,
            IClientRepository clientRepository,
            IDoctorRepository doctorRepository,
            IPlaceRepository placeRepository
            )
        {
            _logger = logger;
            _tokenService = tokenService;
            //_context = context;
            _emailService = emailService;
            _router = routerService;
            _clientRepo = clientRepository;
            _doctorRepo = doctorRepository;
            _placeRepo = placeRepository;

        }

        


        [Route("client/register")]
        [HttpPost]
        public async Task<ActionResult> Register(RegisterDto registerDto)
        {
            Client client = await _clientRepo.GetClientByMedNumberAsync(registerDto.MedNumber);
            Doctor doctor = await _doctorRepo.GetDoctorBySealNumberAsync(registerDto.DoctorSealNumber);
            Place place = await _placeRepo.GetPlaceByPostCodeAndCityAsync(registerDto.PostCode, registerDto.City);
            if (EntityExistsAsync(client))
            {
                return Unauthorized("Ez a TAJ szám már létezik");
            }
            if (!EntityExistsAsync(doctor))
            {
                return Unauthorized("Nem létezik doktor ilyen azonosítóval");
            }
            if (!EntityExistsAsync(place))
            {
                return Unauthorized("Nem létezik ilyen hely");
            }

            client = new Client();
            client.Name = registerDto.Name;
            client.MedNumber = registerDto.MedNumber;
            client.Email = registerDto.Email;
            client.PhoneNumber = registerDto.PhoneNumber;
            HMACSHA512 hmac = new HMACSHA512();
            client.Password = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
            client.PasswordSalt = hmac.Key;
            client.EmailToken = Guid.NewGuid().ToString();
            client.Place = place;
            client.Street = registerDto.Street;
            client.HouseNumber = registerDto.HouseNumber;
            client.BirthDate = DateTime.Parse(registerDto.BirthDate);
            client.Doctor = doctor;
            client.Member = false;
            client.MotherName = registerDto.MotherName;

            _clientRepo.Update(client);

            this._emailService.SuccessfulRegistration(client);

            await _clientRepo.SaveAllAsync();

            return Accepted();
        }

        [Route("client/login")]
        [HttpPost]
        public async Task<ActionResult<ClientDto>> ClientLogin(ClientLoginDto loginDto)
        {
            Client client = await _clientRepo.GetClientByMedNumberAsync(loginDto.MedNumber);

            if (!EntityExistsAsync(client))
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
            HMACSHA512 hmac = new HMACSHA512(client.PasswordSalt);
            byte[] computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != client.Password[i])
                {
                    return Unauthorized("Helytelen jelszó");
                }
            }

            return new ClientDto(client, _tokenService.CreateToken(client));
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
                    user = await _clientRepo.GetClientByMedNumberAsync(lostDto.UserNumber);
                    if (!EntityExistsAsync(user))
                    {
                        return Unauthorized("Nem létező TAJ szám");
                    }
                    user.EmailToken = Guid.NewGuid().ToString() + (char)random.Next(97, 123);
                    _emailService.NewPassword(user);
                    await _clientRepo.SaveAllAsync();
                    return Accepted();
                }
                else if (lostDto.UserNumber.Length == 5)
                {
                    user = await _doctorRepo.GetDoctorBySealNumberAsync(lostDto.UserNumber);
                    if (!EntityExistsAsync(user))
                    {
                        return Unauthorized("Nem létező pecsétszám");
                    }
                    user.EmailToken = Guid.NewGuid().ToString() + (char)random.Next(97, 123);
                    _emailService.NewPassword(user);
                    await _doctorRepo.SaveAllAsync();
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
                User user = await _clientRepo.GetClientByEmailTokenAsync(newDto.EmailToken);
                if (!EntityExistsAsync(user))
                {
                    user = await _doctorRepo.GetDoctorByEmailTokenAsync(newDto.EmailToken);
                    if (!EntityExistsAsync(user))
                    {
                        return Unauthorized("Helytelen azonosító");
                    }
                }
                user.EmailToken = "true";
                HMACSHA512 hmac = new HMACSHA512();
                user.Password = hmac.ComputeHash(Encoding.UTF8.GetBytes(newDto.Password));
                user.PasswordSalt = hmac.Key;
                await _doctorRepo.SaveAllAsync();
                await _clientRepo.SaveAllAsync();//nem biztos, hogy mindkettő kell
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
            
            Client client = await _clientRepo.GetClientByEmailTokenAsync(token);
            if (!EntityExistsAsync(client))
            {
                return Unauthorized("Helytelen azonosító");
            }
            client.EmailToken = "true";
            await _clientRepo.SaveAllAsync();
            return Redirect(_router.Route("/login"));
        }


        [HttpPut]
        [Route("doctor/login")]
        public async Task<ActionResult<DoctorDto>> DoctorLogin(DoctorLoginDto loginDto)
        {
            Doctor doc = await _doctorRepo.GetDoctorBySealNumberAsync(loginDto.SealNumber);
            if (!EntityExistsAsync(doc))
            {
                return Unauthorized("Helytelen pecsétszám szám");
            }
            else if (!(doc.EmailToken.Length == 37 || doc.EmailToken == "true"))
            {
                return Unauthorized("Hitelesítetlen E-mail cím");
            }
            HMACSHA512 hmac = new HMACSHA512(doc.PasswordSalt);
            byte[] computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != doc.Password[i])
                {
                    return Unauthorized("Helytelen jelszó");
                }
            }
            return new DoctorDto(doc, _tokenService.CreateToken(doc));
            //return new { Id = doc.Id, Token = _tokenService.CreateToken(doc) };
        }


        [Route("client/delete/{token}")]
        [HttpGet] //Ez itt igazábol DELETE csak csaltam
        public async Task<ActionResult> DeleteClient(string token)
        {
            Client client = await _clientRepo.GetClientByEmailTokenAsync(token);
            if (!EntityExistsAsync(client) || token == "true")
            {
                return Unauthorized("Helytelen azonosító");
            }
            _clientRepo.DeleteClient(client);
            await _clientRepo.SaveAllAsync();
            return Redirect(_router.Route("/login"));
        }

       private bool EntityExistsAsync(object u)
       {
            if (u == null)
            {
                return false;
            }
            return true;
       }
    }
}
