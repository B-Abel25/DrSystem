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
        [HttpPost]
        [Route("client/post/appointment")]
        public async Task<ActionResult> ClientTakeAppointment(AppointmentDto appDto)
        {
            string clientMedNumber = _tokenService.ReadToken(HttpContext.Request.Headers["Authorization"]);
            Client client = await _clientRepo.GetClientByMedNumberAsync(clientMedNumber);

            if (await HaveAppointment(client))
            {
                return Unauthorized("Egyszerre csak egy foglalás lehet aktív");
            }
            Appointment appointment = new Appointment();
            appointment.AppointmentingUser = client;
            appointment.Doctor = client.Doctor;
            appointment.Description = appDto.Description;
            appointment.Date = appDto.Start;
            appointment.IsDeleted = false;

            _appointmentRepo.PutAppointment(appointment);
            await _appointmentRepo.SaveAllAsync();

            return Accepted();
        }

        [Authorize]
        [HttpPost]
        [Route("doctor/post/appointment")]
        public async Task<ActionResult> DoctorTakeAppointment(AppointmentDto appDto)
        {
            string clientMedNumber = _tokenService.ReadToken(HttpContext.Request.Headers["Authorization"]);
            Client client = await _clientRepo.GetClientByMedNumberAsync(clientMedNumber);

            if (await HaveAppointment(client))
            {
                return Unauthorized("Egyszerre csak egy foglalás lehet aktív");
            }

            Appointment appointment = new Appointment();
            appointment.AppointmentingUser = client;
            appointment.Doctor = client.Doctor;
            appointment.Description = appDto.Description;
            appointment.Date = appDto.Start;
            appointment.IsDeleted = false;

            _appointmentRepo.PutAppointment(appointment);
            await _appointmentRepo.SaveAllAsync();

            return Accepted();
        }

        [Authorize]
        [HttpGet]
        [Route("doctor/get/appointments")]
        public async Task<ActionResult<List<AppointmentDto>>> GetClientsAppointmentsToDoctor()
        {
            string doctorSealNumber = _tokenService.ReadToken(HttpContext.Request.Headers["Authorization"]);
            Doctor doctor = await _doctorRepo.GetDoctorBySealNumberAsync(doctorSealNumber);

            List<Appointment> docApps = await _appointmentRepo.GetAppointmentsByDoctorAsync(doctor);

            List<AppointmentDto> Dtos = new List<AppointmentDto>();
            foreach (var docApp in docApps)
            {
                if (!docApp.IsDeleted)
                {
                    Dtos.Add(new AppointmentDto(docApp));
                }
            }

            return Dtos;
        }

        [Authorize]
        [HttpGet]
        [Route("client/get/appointments")]
        public async Task<ActionResult<List<AppointmentDto>>> GetClientsAppointmentsToClient()
        {
            string clientMedNumber = _tokenService.ReadToken(HttpContext.Request.Headers["Authorization"]);
            Client client = await _clientRepo.GetClientByMedNumberAsync(clientMedNumber);

            

            List<Appointment> docApps = await _appointmentRepo.GetAppointmentsByDoctorAsync(client.Doctor);

            List<AppointmentDto> Dtos = new List<AppointmentDto>();
            foreach (var docApp in docApps)
            {
                if (!docApp.IsDeleted)
                {
                    AppointmentDto d = new AppointmentDto(docApp);
                    d.Title = "Foglalt";
                    d.Description = "";
                    Dtos.Add(d);
                }
            }

            return Dtos;
        }

        private async Task<bool> HaveAppointment(Client client)
        {
            List<Appointment> apps = await _appointmentRepo.GetAppointmentsByClientAsync(client);
            foreach (var app in apps)
            {
                if (app.Date > DateTime.Now && !app.IsDeleted)
                {
                    return true;
                }
            }
            return false;
        }

    }
}
