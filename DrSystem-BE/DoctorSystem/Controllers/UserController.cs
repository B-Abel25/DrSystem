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
    [Route("user")]
    public class UserController : ControllerBase
    {


        private readonly ILogger<AccountController> _logger;
        private readonly BaseDbContext _context;
        private readonly ITokenService _tokenService;
        private readonly EmailService _emailService;

        public UserController(ILogger<AccountController> logger, BaseDbContext context, ITokenService tokenService, EmailService emailService)
        {
            _logger = logger;
            //_accountService = registerService;
            _tokenService = tokenService;
            _context = context;
            _emailService = emailService;
        }

        [Route("doctors")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetDoctors()
        {
            var doctors =  await _context._doctors.Include(d => d.Place.City.County).ToListAsync();

            List<object> result = new List<object>();
            foreach (var doctor in doctors)
            {
                result.Add(new {Id = doctor.Id, Name = doctor.Name, PostCode = doctor.Place.PostCode });
            }
            return result;
        }

        [Route("places")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Place>>> GetPlaces()
        {
            return await _context._place.Include(p => p.City.County).ToListAsync();
        }
    }
}
