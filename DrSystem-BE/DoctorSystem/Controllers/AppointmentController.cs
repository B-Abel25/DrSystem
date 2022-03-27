using DoctorSystem.Dtos;
using DoctorSystem.Entities;
using DoctorSystem.Interfaces;
using DoctorSystem.Model.Enums;
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
        private readonly IOfficeHoursRepository _officeHoursRepo;
        private readonly Regex sWhitespace = new Regex(@"\s+");

        public AppointmentController(
            ILogger<AppointmentController> logger,
            ITokenService tokenService,
            EmailService emailService,
            IClientRepository clientRepository,
            IDoctorRepository doctorRepository,
            IAppointmentRepository appointmentRepo,
            IOfficeHoursRepository officeHoursRepo
            )
        {
            _logger = logger;
            _tokenService = tokenService;
            _emailService = emailService;
            _appointmentRepo = appointmentRepo;
            _clientRepo = clientRepository;
            _doctorRepo = doctorRepository;
            _officeHoursRepo = officeHoursRepo;
        }

       
        [Authorize]
        [HttpPost]
        [Route("client/post/appointment")]
        public async Task<ActionResult> ClientTakeAppointment(AppointmentDto appDto)
        {
            string clientMedNumber = _tokenService.ReadToken(HttpContext.Request.Headers["Authorization"]);
            Client client = await _clientRepo.GetClientByMedNumberAsync(clientMedNumber);

            if (appDto.Start < DateTime.Now)
            {
                return Unauthorized("Ez az időpont már elmúlt Próbáljon későbbi időpontot foglalni");
            }
            List<Appointment> apps = await _appointmentRepo.GetAppointmentsByDoctorAsync(client.Doctor);
            OfficeHours oh = await _officeHoursRepo.GetOfficeHoursByDoctorAndDay(client.Doctor, (Days)((int)appDto.Start.DayOfWeek));        
            
            if (!(appDto.Start >= oh.Open && appDto.Start <= oh.Close.AddMinutes(client.Doctor.Duration)))
            {
                return Unauthorized("Az időpont az orvos rendelési idején kívülre esik");
            }
                        
            foreach (var app in apps)
            {
                if (!(appDto.Start > app.Date && appDto.Start < app.Date.AddMinutes(client.Doctor.Duration)))
                {
                    return Unauthorized("Ez az időpont már le van foglalva, válasszon másikat");
                }
            }

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
            string doctorSealNumber = _tokenService.ReadToken(HttpContext.Request.Headers["Authorization"]);
            Doctor doctor = await _doctorRepo.GetDoctorBySealNumberAsync(doctorSealNumber);

            if (appDto.Start < DateTime.Now)
            {
                return Unauthorized("Ez az időpont már elmúlt Próbáljon későbbi időpontot foglalni");
            }

            List<Appointment> apps = await _appointmentRepo.GetAppointmentsByDoctorAsync(doctor);
            foreach (var app in apps)
            {
                if (appDto.Start > app.Date && appDto.Start < app.Date.AddMinutes(doctor.Duration))
                {
                    return Unauthorized("Ez az időpont már le van foglalva, válasszon másikat");
                }
            }

            OfficeHours oh = await _officeHoursRepo.GetOfficeHoursByDoctorAndDay(doctor, (Days)((int)appDto.Start.DayOfWeek));
            
            if ((oh.Open == DateTime.MinValue && oh.Close == DateTime.MinValue) ||appDto.Start >= oh.Open && appDto.Start <= oh.Close.AddMinutes(-doctor.Duration))
            { 
                return Unauthorized("Az időpont rendelési időn kívülre esik");
            }
            
            

            Appointment appointment = new Appointment();
            appointment.AppointmentingUser = doctor;
            appointment.Doctor = doctor;
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
                AppointmentDto d = new AppointmentDto(docApp);
                if (!docApp.IsDeleted && docApp.AppointmentingUser.Id != doctor.Id)
                {
                    d.Title = "Foglalt";
                    d.Description = "";
                    d.Color = "blue";
                }
                else
                {
                    d.Color = "purple";
                }
                Dtos.Add(d);
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
                AppointmentDto d = new AppointmentDto(docApp);
                if (!docApp.IsDeleted && docApp.AppointmentingUser.Id != client.Id)
                {
                   
                    d.Title = "Foglalt";
                    d.Description = "";
                    d.Color = "blue";
                    
                }
                else
                {
                    d.Color = "green";
                }
                Dtos.Add(d);
            }

            return Dtos;
        }

        [Authorize]
        [HttpDelete]
        [Route("client/delete/appointment")]
        public async Task<ActionResult> DeleteClientAppointment()
        {
            string clientMedNumber = _tokenService.ReadToken(HttpContext.Request.Headers["Authorization"]);
            Client client = await _clientRepo.GetClientByMedNumberAsync(clientMedNumber);

            List<Appointment> docApps = await _appointmentRepo.GetAppointmentsByClientAsync(client);

            foreach (var docApp in docApps)
            {
                if (docApp.Date > DateTime.Now)
                {
                    docApp.IsDeleted = true;
                }
            }
            await _appointmentRepo.SaveAllAsync();
            return Accepted();
        }

        [Authorize]
        [HttpDelete]
        [Route("doctor/delete/appointment")]
        public async Task<ActionResult> DeleteDoctorAppointment(AppointmentDto dto)
        {
            string doctorSealNumber = _tokenService.ReadToken(HttpContext.Request.Headers["Authorization"]);
            Doctor doctor = await _doctorRepo.GetDoctorBySealNumberAsync(doctorSealNumber);

            List<Appointment> docApps = await _appointmentRepo.GetAppointmentsByDoctorAsync(doctor);

            foreach (var docApp in docApps)
            {
                if (docApp.Date > DateTime.Now && dto.Start == docApp.Date)
                {
                    docApp.IsDeleted = true;
                }
            }
            await _appointmentRepo.SaveAllAsync();
            return Accepted();
        }

        [Authorize]
        [HttpGet]
        [Route("oneClient/appointments/{medNumber}")]
        public async Task<ActionResult<List<AppointmentDto>>> GetOneClientAppointments(string medNumber)
        {
            Client c = await _clientRepo.GetClientByMedNumberAsync(medNumber);
            if (c == null)
            {
                return Unauthorized("Nincs ilyen TAJ számmal rendelkező páciens");
            }
            string userNumber = _tokenService.ReadToken(HttpContext.Request.Headers["Authorization"]);
            Doctor doctor;
            if (userNumber != medNumber && medNumber.Length == 9)
            {
                 doctor = await _doctorRepo.GetDoctorBySealNumberAsync(userNumber);
                if (!doctor.Clients.Contains(c))
                {
                    return Unauthorized("Ez a páciens nem önhöz tartozik");
                }
            }

            List<Appointment> apps = await _appointmentRepo.GetAppointmentsByClientAsync(c);
            List<AppointmentDto> appDtos = new List<AppointmentDto>();

            foreach (var app in apps)
            {
                appDtos.Add(new AppointmentDto(app));
            }
            return appDtos;
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
