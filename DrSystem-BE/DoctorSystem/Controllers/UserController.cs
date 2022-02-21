using DoctorSystem.Dtos;
using DoctorSystem.Entities.Contexts;
using DoctorSystem.Interfaces;
using DoctorSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DoctorSystem.Controller
{
    [ApiController]
    [Route("private/")]
    public class UserController
    {
        private readonly ILogger<UserController> _logger;
        private readonly BaseDbContext _context;
        private readonly ITokenService _tokenService;
        private readonly EmailService _emailService;

        public UserController(ILogger<UserController> logger, BaseDbContext context, ITokenService tokenService, EmailService emailService)
        {
            _logger = logger;
            //_accountService = registerService;
            _tokenService = tokenService;
            _context = context;
            _emailService = emailService;
        }

        
        [Route("doctor/clients/{doctorId}")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClientDto>>> GetClientsByDoctorId(string doctorId)
        {
            var clients = await _context._clients.Include(c => c.Doctor.Place.City.County).Include(c => c.Place.City.County).ToListAsync();

            List<ClientDto> clientDtos = new List<ClientDto>();
            foreach (var client in clients)
            {
                if (client.Doctor.Id == doctorId && client.Member)
                {
                    clientDtos.Add(new ClientDto(client));
                }
            }
            return clientDtos;

        }

        
        [Route("doctor/client-requests/{doctorId}")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClientDto>>> GetClientsByDoctorIdAndNotMember(string doctorId)
        {
            var clients = await _context._clients.Include(c => c.Doctor.Place.City.County).Include(c => c.Place.City.County).ToListAsync();

            List<ClientDto> clientDtos = new List<ClientDto>();
            foreach (var client in clients)
            {
                if (client.Doctor.Id == doctorId && !client.Member)
                {
                    clientDtos.Add(new ClientDto(client));
                }
            }
            return clientDtos;

        }
    }
}
