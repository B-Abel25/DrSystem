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
using System.Text;
using System.Threading.Tasks;

namespace DoctorSystem.Controller
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
        private readonly EmailService _emailService;

        public UserController(
            ILogger<UserController> logger,
            BaseDbContext context,
            ITokenService tokenService,
            IClientRepository clientRepository,
            IDoctorRepository doctorRepository,
            EmailService emailService
            )
        {
            _logger = logger;
            _tokenService = tokenService;
            //_context = context;
            _emailService = emailService;
            _clientRepo = clientRepository;
            _doctorRepo = doctorRepository;
        }

        [Authorize]
        [Route("doctor/clients")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClientDto>>> GetClientsByDoctorId()
        {
            string doctorSealNumber = _tokenService.ReadToken(HttpContext.Request.Headers["Authorization"]);
            Doctor doctor = await _doctorRepo.GetDoctorBySealNumberAsync(doctorSealNumber);

            List<Client> clients = await _clientRepo.GetClientsAsync();

            List<ClientDto> clientDtos = new List<ClientDto>();
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
        public async Task<ActionResult<IEnumerable<ClientDto>>> GetClientsByDoctorIdAndNotMember()
        {
            string doctorSealNumber = _tokenService.ReadToken(HttpContext.Request.Headers["Authorization"]);
            Doctor doctor = await _doctorRepo.GetDoctorBySealNumberAsync(doctorSealNumber);
            List<Client> clients = await _clientRepo.GetClientsAsync();

            List<ClientDto> clientDtos = new List<ClientDto>();
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
            //TODO elutasítás validáció
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

       
    }
}
