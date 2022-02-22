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
    public class UserController : ControllerBase
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

        [Authorize]
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

        
        [Authorize]
        [Route("doctor/clients-request/{doctorId}")]
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

        [Authorize()]
        [Route("doctor/client-request/accept/{clientId}")]
        [HttpPut]
        public async Task<ActionResult> AcceptClientRequest(string clientId)
        {
            //TODO email éretsítés az elfogadásról
            var client = await _context._clients.SingleOrDefaultAsync(x=> x.Id == clientId);
            client.Member = true;
            await _context.SaveChangesAsync();
            return Accepted();
        }


        [Authorize]
        [Route("doctor/client-request/decline/{clientId}")]
        [HttpDelete]
        public async Task<ActionResult> DeclineClientRequest(string clientId)
        {
            //TODO email éretsítés az elutasításról
            var client = await _context._clients.SingleOrDefaultAsync(x => x.Id == clientId);
            _context._clients.Remove(client);
            await _context.SaveChangesAsync();
            return Accepted();
        }
    }
}
