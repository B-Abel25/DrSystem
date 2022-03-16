using DoctorSystem.Interfaces;
using DoctorSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace DoctorSystem.Controllers
{
    [Route("private/")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly ILogger<AppointmentController> _logger;
        private readonly ITokenService _tokenService;
        private readonly EmailService _emailService;
        private readonly IMessageRepository _messageRepo;
        private readonly IClientRepository _clientRepo;
        private readonly IDoctorRepository _doctorRepo;
        private readonly Regex sWhitespace = new Regex(@"\s+");
        public AppointmentController(
            ILogger<AppointmentController> logger,
            ITokenService tokenService,
            EmailService emailService,
            IMessageRepository messageRepository,
            IClientRepository clientRepository,
            IDoctorRepository doctorRepository
            )
        {
            _logger = logger;
            _tokenService = tokenService;
            _emailService = emailService;
            _messageRepo = messageRepository;
            _clientRepo = clientRepository;
            _doctorRepo = doctorRepository;
        }



    }
}
