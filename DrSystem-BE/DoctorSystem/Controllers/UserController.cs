﻿using DoctorSystem.Dtos;
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
        [Route("doctor/clients")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClientDto>>> GetClientsByDoctorId()
        {
            string doctorId = _tokenService.ReadToken(HttpContext.Request.Headers["Authorization"]);

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
        [Route("doctor/clients-request")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClientDto>>> GetClientsByDoctorIdAndNotMember()
        {
            string doctorId = _tokenService.ReadToken(HttpContext.Request.Headers["Authorization"]);

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

        [Authorize]
        [Route("doctor/client-request/accept/{clientId}")]
        [HttpPut]
        public async Task<ActionResult> AcceptClientRequest(string clientId)
        {
            //Peti review: clientId küldést esetleg lehetne tokenbe?
            //TODO email éretsítés az elfogadásról
            string doctorId = _tokenService.ReadToken(HttpContext.Request.Headers["Authorization"]);
            var doctor = await _context._doctors.Include(x => x.Clients).SingleOrDefaultAsync(x => x.Id == doctorId);
            var client = await _context._clients.SingleOrDefaultAsync(x => x.Id == clientId);

            foreach (var doctorClient in doctor.Clients)
            {
                if (doctorClient.Id == client.Id)
                {
                    client.Member = true;
                    await _context.SaveChangesAsync();
                    return Accepted();
                }
            }
            return Unauthorized("asztapaszta");
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

        #region FileUploadController
        /*
        [HttpPost]
        public async Task<IActionResult> OnPostUploadAsync(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    var filePath = Path.GetTempFileName();

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }

            // Process uploaded files
            // Don't rely on or trust the FileName property without validation.

            return Ok(new { count = files.Count, size });
        }
        */
        #endregion

        [Authorize]
        [Route("user/messages/{userId}")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessagesById()
        {
            return null;
        }
    }
}
