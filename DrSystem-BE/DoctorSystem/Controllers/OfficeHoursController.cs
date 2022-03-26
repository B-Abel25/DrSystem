using DoctorSystem.Interfaces;
using DoctorSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using DoctorSystem.Dtos;
using DoctorSystem.Entities;
using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace DoctorSystem.Controllers
{
    [Route("private/")]
    [ApiController]
    public class OfficeHoursController : ControllerBase
    {
        private readonly ILogger<OfficeHoursController> _logger;
        private readonly ITokenService _tokenService;
        private readonly EmailService _emailService;
        IDoctorRepository _doctorRepo;
        IOfficeHoursRepository _officeHoursRepo;

        public OfficeHoursController(
           ILogger<OfficeHoursController> logger,
           ITokenService tokenService,
           EmailService emailService,
           IDoctorRepository doctorRepository
           )
        {
            _logger = logger;
            _tokenService = tokenService;
            _emailService = emailService;
            _doctorRepo = doctorRepository;
        }


        [Authorize]
        [HttpGet]
        [Route("doctor/office-hours")]
        public async Task<ActionResult<IEnumerable<OfficeHoursDto>>> GetDoctorOfficeHours()
        {
            string doctorSealNumber = _tokenService.ReadToken(HttpContext.Request.Headers["Authorization"]);
            Doctor doctor = await _doctorRepo.GetDoctorBySealNumberAsync(doctorSealNumber);

            List<OfficeHours> ohs = await _officeHoursRepo.GetOfficeHoursAllDayByDoctor(doctor);
            List<OfficeHoursDto> ohDtos = new List<OfficeHoursDto>();
            foreach (var oh in ohs)
            {
                ohDtos.Add(new OfficeHoursDto(oh));
            }
            return ohDtos;
        }

        [Authorize]
        [HttpPut]
        [Route("doctor/office-hours/modify")]
        public async Task<ActionResult> ModifyDoctorOfficeHours(List<ModifyOfficeHoursDto> modifyDto)
        {
            if (modifyDto.Count != 5)
            {
                return Unauthorized("Nem 5 objektum érkezett");
            }
            modifyDto = modifyDto.OrderBy(x => x.Day).ToList();
            string doctorSealNumber = _tokenService.ReadToken(HttpContext.Request.Headers["Authorization"]);
            Doctor doctor = await _doctorRepo.GetDoctorBySealNumberAsync(doctorSealNumber);

            RemoveOfficeHours(doctor);

            foreach (var modify in modifyDto)
            {
                OfficeHours oh = new OfficeHours();
                oh.Day = modify.Day;
                oh.Opening = DateTime.Parse(modify.Open);
                oh.Closing = DateTime.Parse(modify.Close);
                oh.Doctor = doctor;
                _officeHoursRepo.Update(oh);
            }
            await _officeHoursRepo.SaveAllAsync();

            return Accepted();
        }
        private async void RemoveOfficeHours(Doctor doctor)
        {
            List<OfficeHours> oh = await _officeHoursRepo.GetOfficeHoursAllDayByDoctor(doctor);

            foreach (var item in oh)
            {
                _officeHoursRepo.RemoveOfficeHour(item);
            }
            await _officeHoursRepo.SaveAllAsync();
        }

        [Authorize]
        [HttpGet]
        [Route("doctor/get/duration")]
        public async Task<ActionResult<int>> GetDoctorDuration()
        {
            string doctorSealNumber = _tokenService.ReadToken(HttpContext.Request.Headers["Authorization"]);
            Doctor doctor = await _doctorRepo.GetDoctorBySealNumberAsync(doctorSealNumber);
            return doctor.Duration;
        }

        [Authorize]
        [HttpPut]
        [Route("doctor/put/duration/{duration:int}")]
        public async Task<ActionResult> PutDoctorDuration(int duration)
        {
            string doctorSealNumber = _tokenService.ReadToken(HttpContext.Request.Headers["Authorization"]);
            Doctor doctor = await _doctorRepo.GetDoctorBySealNumberAsync(doctorSealNumber);
            doctor.Duration = duration;
            await _doctorRepo.SaveAllAsync();
            return Accepted();
        }
    }
}
