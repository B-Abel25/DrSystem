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
        //private readonly BaseDbContext _context;
        private readonly ITokenService _tokenService;
        private readonly EmailService _emailService;
        IDoctorRepository _doctorRepo;
        IPlaceRepository _placeRepo;

        public RegisterController(
            ILogger<RegisterController> logger,
            BaseDbContext context,
            ITokenService tokenService,
            EmailService emailService,
            IDoctorRepository doctorRepository,
            IPlaceRepository placeRepo
            )
        {
            _logger = logger;
            _tokenService = tokenService;
            //_context = context;
            _emailService = emailService;
            _doctorRepo = doctorRepository;
            _placeRepo = placeRepo;
        }

        [Route("doctors")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DoctorDto>>> GetDoctors()
        {
            List<Doctor> doctors = await _doctorRepo.GetDoctorsAsync();

            List<DoctorDto> result = new List<DoctorDto>();
            foreach (var doctor in doctors)
            {
                result.Add(new DoctorDto()
                {
                    Name = doctor.Name,
                    SealNumber = doctor.SealNumber,
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
            List<Place> places = await _placeRepo.GetPlacesAsync();
            List<PlaceDto> placeDtos = new List<PlaceDto>();
            places.ForEach(x => placeDtos.Add(new PlaceDto(x)));
            return placeDtos;
        }
    }
}
