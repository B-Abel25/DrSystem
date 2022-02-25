using DoctorSystem.Dtos;
using DoctorSystem.Entities;
using DoctorSystem.Entities.Contexts;
using DoctorSystem.Interfaces;
using DoctorSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DoctorSystem.Controllers
{
    [ApiController]
    [Route("public/register")]
    public class RegisterController : ControllerBase
    {


        private readonly ILogger<RegisterController> _logger;
        private readonly BaseDbContext _context;
        private readonly ITokenService _tokenService;
        private readonly EmailService _emailService;

        public RegisterController(ILogger<RegisterController> logger, BaseDbContext context, ITokenService tokenService, EmailService emailService)
        {
            _logger = logger;
            _tokenService = tokenService;
            _context = context;
            _emailService = emailService;
        }

        [Route("doctors")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DoctorDto>>> GetDoctors()
        {
            var doctors =  await _context._doctors.Include(d => d.Place).ToListAsync();

            List<DoctorDto> result = new List<DoctorDto>();
            foreach (var doctor in doctors)
            {

                result.Add(new DoctorDto()
                {
                    Id = doctor.Id,
                    Name = doctor.Name,
                    Place = new PlaceDto() 
                    {
                        PostCode = doctor.Place.PostCode
                    } 
                });
            }
            return result;
        }

        [Route("places")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlaceDto>>> GetPlaces()
        {
            var places = await _context._place.Include(p => p.City.County).ToListAsync();
            List<PlaceDto> placeDtos = new List<PlaceDto>();
            foreach (var place in places)
            {
                placeDtos.Add(new PlaceDto(place));
            }
            return placeDtos;
        }
    }
}
