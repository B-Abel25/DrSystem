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
        IClientRepository _clientRepo;

        public OfficeHoursController(
           ILogger<OfficeHoursController> logger,
           ITokenService tokenService,
           EmailService emailService,
           IDoctorRepository doctorRepository,
           IClientRepository clientRepository,
           IOfficeHoursRepository officeHoursRepository
           )
        {
            _logger = logger;
            _tokenService = tokenService;
            _emailService = emailService;
            _doctorRepo = doctorRepository;
            _officeHoursRepo = officeHoursRepository;
            _clientRepo = clientRepository;
        }


        [Authorize]
        [HttpGet]
        [Route("doctor/office-hours")]
        public async Task<ActionResult<List<OfficeHoursDto>>> GetDoctorOfficeHours()
        {
            string doctorSealNumber = _tokenService.ReadToken(HttpContext.Request.Headers["Authorization"]);
            Doctor doctor = await _doctorRepo.GetDoctorBySealNumberAsync(doctorSealNumber);

            List<OfficeHours> ohs = await _officeHoursRepo.GetOfficeHoursAllDayByDoctor(doctor);
            List<OfficeHoursDto> ohDtos = new List<OfficeHoursDto>();
            foreach (var oh in ohs)
            {
                ohDtos.Add(new OfficeHoursDto(oh));
            }
            return ohDtos.OrderBy(x => x.Day).ToList();
        }

        [Authorize]
        [HttpGet]
        [Route("client/office-hours")]
        public async Task<ActionResult<List<OfficeHoursDto>>> GetClientOfficeHours()
        {
            string clientMednumber = _tokenService.ReadToken(HttpContext.Request.Headers["Authorization"]);
            Client client = await _clientRepo.GetClientByMedNumberAsync(clientMednumber);

            List<OfficeHours> ohs = await _officeHoursRepo.GetOfficeHoursAllDayByDoctor(client.Doctor);
            List<OfficeHoursDto> ohDtos = new List<OfficeHoursDto>();
            foreach (var oh in ohs)
            {
                ohDtos.Add(new OfficeHoursDto(oh));
            }
            return ohDtos.OrderBy(x => x.Day).ToList();
        }

        [Authorize]
        [HttpPut]
        [Route("doctor/office-hours/modify")]
        public async Task<ActionResult> ModifyDoctorOfficeHours(List<OfficeHoursDto> modifyDtos)
        {
            if (modifyDtos.Count != 5)
            {
                return Unauthorized("Nem 5 objektum érkezett");
            }
            string doctorSealNumber = _tokenService.ReadToken(HttpContext.Request.Headers["Authorization"]);
            Doctor doctor = await _doctorRepo.GetDoctorBySealNumberAsync(doctorSealNumber);
            foreach (var modifyDto in modifyDtos)
            {
                if (modifyDto.Open == "" && modifyDto.Close != "")
                {
                    return Unauthorized("Ha van megadva zárási idő akkor a nyitási időt is meg kell adni");
                }
                if (modifyDto.Open != "" && modifyDto.Close == "")
                {
                    return Unauthorized("Ha van megadva nyitási idő akkor a zárási időt is meg kell adni");
                }
                if ((modifyDto.Open != "" && modifyDto.Close != "") && (DateTime.Parse(modifyDto.Open).AddMinutes(doctor.Duration) > DateTime.Parse(modifyDto.Close)))
                {
                    return Unauthorized("A nyitási időnek legalább egy foglalásnyiidővel kebesebbnek kell lennie a zárásnál");
                }  
            }
            modifyDtos = modifyDtos.OrderBy(x => x.Day).ToList();


            //TODO ha van foglálas olyan időpontra akkor ne lehessen megváltoztatni
            await RemoveOfficeHours(doctor);

            foreach (var modifyDto in modifyDtos)
            {
                OfficeHours oh = new OfficeHours();
                oh.Day = modifyDto.Day;
                if (modifyDto.Open != "")
                {
                    oh.Open = DateTime.Parse(modifyDto.Open);
                }
                if (modifyDto.Close != "")
                {
                    oh.Close = DateTime.Parse(modifyDto.Close);
                }     
                oh.Doctor = doctor;
                _officeHoursRepo.Update(oh);
            }
            await _officeHoursRepo.SaveAllAsync();

            return Accepted();
        }
       
        private async Task<bool> RemoveOfficeHours(Doctor doctor)
        {
            List<OfficeHours> oh = await _officeHoursRepo.GetOfficeHoursAllDayByDoctor(doctor);

            foreach (var item in oh)
            {
                _officeHoursRepo.RemoveOfficeHour(item);
            }
            await _officeHoursRepo.SaveAllAsync();
            return true;
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
        [HttpGet]
        [Route("client/get/duration")]
        public async Task<ActionResult<int>> GetClientDuration()
        {
            string clientMedNumber = _tokenService.ReadToken(HttpContext.Request.Headers["Authorization"]);
            Client client = await _clientRepo.GetClientByMedNumberAsync(clientMedNumber);
            return client.Doctor.Duration;
        }

        [Authorize]
        [HttpPut]
        [Route("doctor/put/duration/{duration}")]
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
