using DoctorSystem.Dtos;
using DoctorSystem.Entities;
using DoctorSystem.Interfaces;
using DoctorSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DoctorSystem.Controllers
{
    [Route("private/")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly ILogger<AppointmentController> _logger;
        private readonly ITokenService _tokenService;
        private readonly EmailService _emailService;
        private readonly IClientRepository _clientRepo;
        private readonly IDoctorRepository _doctorRepo;
        private readonly IAppointmentRepository _appointmentRepo;
        private readonly Regex sWhitespace = new Regex(@"\s+");
        public AppointmentController(
            ILogger<AppointmentController> logger,
            ITokenService tokenService,
            EmailService emailService,
            IClientRepository clientRepository,
            IDoctorRepository doctorRepository,
            IAppointmentRepository appointmentRepo
            )
        {
            _logger = logger;
            _tokenService = tokenService;
            _emailService = emailService;
            _appointmentRepo = appointmentRepo;
            _clientRepo = clientRepository;
            _doctorRepo = doctorRepository;
        }

        [Authorize]
        [HttpPut]
        [Route("client/put/appointment")]
        public async Task<ActionResult> TakeAppointment(AppointmentDto appDto)
        {
            string clientMedNumber = _tokenService.ReadToken(HttpContext.Request.Headers["Authorization"]);
            Client client = await _clientRepo.GetClientByMedNumberAsync(clientMedNumber);

            Appointment appointment = new Appointment();
            appointment.Client = client;
            appointment.Doctor = client.Doctor;
            appointment.Description = appDto.Description;
            appointment.Date = appDto.Date;

            _appointmentRepo.PutAppointment(appointment);
            await _appointmentRepo.SaveAllAsync();

            return Accepted();
        }

        [Authorize]
        [HttpPut]
        [Route("doctor/get/appointments")]
        public async Task<ActionResult<List<AppointmentDto>>> GetClientAppointments()
        {
            string doctorSealNumber = _tokenService.ReadToken(HttpContext.Request.Headers["Authorization"]);
            Doctor doctor = await _doctorRepo.GetDoctorBySealNumberAsync(doctorSealNumber);

            List<Appointment> docApps = await _appointmentRepo.GetAppointmentsByDoctor(doctor);

            List<AppointmentDto> Dtos = new List<AppointmentDto>();
            foreach (var docApp in docApps)
            {
                Dtos.Add(new AppointmentDto(docApp));
            }

            return Dtos;
        }



    }
}
