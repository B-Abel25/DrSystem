using DoctorSystem.Dtos;
using DoctorSystem.Entities;
using DoctorSystem.Entities.Contexts;
using DoctorSystem.Interfaces;
using DoctorSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DoctorSystem.Controllers
{
    [ApiController]
    [Route("private/")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        //private readonly BaseDbContext _context;
        private readonly ITokenService _tokenService;
        private readonly IClientRepository _clientRepo;
        private readonly IDoctorRepository _doctorRepo;
        private readonly IPlaceRepository _placeRepo;
        private readonly EmailService _emailService;

        public UserController(
            ILogger<UserController> logger,
            BaseDbContext context,
            ITokenService tokenService,
            IClientRepository clientRepository,
            IDoctorRepository doctorRepository,
            EmailService emailService,
            IPlaceRepository placeRepo
            )
        {
            _logger = logger;
            _tokenService = tokenService;
            //_context = context;
            _emailService = emailService;
            _clientRepo = clientRepository;
            _doctorRepo = doctorRepository;
            _placeRepo = placeRepo;
        }

        [Authorize]
        [Route("doctor/clients")]
        [HttpGet]
        public async Task<ActionResult<List<ClientDto>>> GetClientsByDoctorId()
        {
            string doctorSealNumber = _tokenService.ReadToken(HttpContext.Request.Headers["Authorization"]);
            Doctor doctor = await _doctorRepo.GetDoctorBySealNumberAsync(doctorSealNumber);

            List<Client> clients = await _clientRepo.GetClientsAsync();

            List<ClientDto> clientDtos = new List<ClientDto>();
            clients.OrderBy(x => x.CreateDate);
            foreach (var client in clients)
            {
                if (client.Doctor.Id == doctor.Id && client.Member)
                {
                    clientDtos.Add(new ClientDto(client));
                }
            }
            return clientDtos;

        }
        
        [Authorize]
        [Route("doctor/clients-request")]
        [HttpGet]
        public async Task<ActionResult<List<ClientDto>>> GetClientsByDoctorIdAndNotMember()
        {
            string doctorSealNumber = _tokenService.ReadToken(HttpContext.Request.Headers["Authorization"]);
            Doctor doctor = await _doctorRepo.GetDoctorBySealNumberAsync(doctorSealNumber);
            List<Client> clients = await _clientRepo.GetClientsAsync();

            List<ClientDto> clientDtos = new List<ClientDto>();
            clients.OrderBy(x => x.CreateDate);
            foreach (var client in clients)
            {
                if (client.Doctor.Id == doctor.Id && !client.Member)
                {
                    clientDtos.Add(new ClientDto(client));
                }
            }
            return clientDtos;

        }

        [Authorize]
        [Route("doctor/client-request/accept/{medNumber}")]
        [HttpPut]
        public async Task<ActionResult> AcceptClientRequest(string medNumber)
        {
            //TODO email éretsítés az elfogadásról
            string doctorSealNumber = _tokenService.ReadToken(HttpContext.Request.Headers["Authorization"]);
            Doctor doctor = await _doctorRepo.GetDoctorBySealNumberAsync(doctorSealNumber);
            Client client = await _clientRepo.GetClientByMedNumberAsync(medNumber);

            if (doctor.Clients.Contains(client))
            {
                client.Member = true;
                await _clientRepo.SaveAllAsync();
                return Accepted();
            }
            return Unauthorized("asztapaszta");
        }

        [Authorize]
        [Route("doctor/client-request/decline/{medNumber}")]
        [HttpDelete]
        public async Task<ActionResult> DeclineClientRequest(string medNumber)
        {
            //TODO email éretsítés az elutasításról
            string doctorSealNumber = _tokenService.ReadToken(HttpContext.Request.Headers["Authorization"]);
            Doctor doctor = await _doctorRepo.GetDoctorBySealNumberAsync(doctorSealNumber);
            Client client = await _clientRepo.GetClientByMedNumberAsync(medNumber);
            if (doctor.Clients.Contains(client))
            {
                _clientRepo.DeleteClient(client);
                await _clientRepo.SaveAllAsync();
                return Accepted();
            }
            return Unauthorized("asztapaszta");
        }

        [Authorize]
        [Route("client/get/me")]
        [HttpGet]
        public async Task<ActionResult<ClientDto>> GetClientToClientByMedNumber()
        {
            string clientMedNumber = _tokenService.ReadToken(HttpContext.Request.Headers["Authorization"]);
            Client client = await _clientRepo.GetClientByMedNumberAsync(clientMedNumber);

            return new ClientDto(client);
        }

        [Authorize]
        [Route("doctor/get/client/{medNumber}")]
        [HttpGet]
        public async Task<ActionResult<ClientDto>> GetDoctorToClientByMedNumber(string medNumber)
        {
            string doctorSealNumber = _tokenService.ReadToken(HttpContext.Request.Headers["Authorization"]);
            Doctor doctor = await _doctorRepo.GetDoctorBySealNumberAsync(doctorSealNumber);
 
            return new ClientDto(await _clientRepo.GetClientByIdAsync(doctor.Clients.First(x => x.MedNumber == medNumber).Id));
        }

        [Route("client/register/modify")]
        [HttpPut]
        public async Task<ActionResult> RegisterModify(RegisterDto registerDto)
        {
            Client client = await _clientRepo.GetClientByMedNumberAsync(registerDto.MedNumber);
            Place place = await _placeRepo.GetPlaceByPostCodeAndCityAsync(registerDto.PostCode, registerDto.City);
            City birthPlace = await _placeRepo.GetCityByNameAsync(registerDto.BirthPlace);

            client.Name = registerDto.Name;
            client.MedNumber = registerDto.MedNumber;
            client.Email = registerDto.Email;
            client.PhoneNumber = registerDto.PhoneNumber;
            HMACSHA512 hmac = new HMACSHA512();
            client.Password = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
            client.PasswordSalt = hmac.Key;
            client.Place = place;
            client.Street = registerDto.Street;
            client.HouseNumber = registerDto.HouseNumber;
            client.BirthDate = DateTime.Parse(registerDto.BirthDate);
            client.MotherName = registerDto.MotherName;
            client.BirthPlace = birthPlace;

            _clientRepo.Update(client);
            await _clientRepo.SaveAllAsync();

            return Accepted();
        }

    }
}
