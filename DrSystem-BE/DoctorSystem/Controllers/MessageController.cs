using DoctorSystem.Dtos;
using DoctorSystem.Entities;
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
using System.Threading.Tasks;

namespace DoctorSystem.Controllers
{
    [ApiController]
    [Route("private/")]
    public class MessageController : ControllerBase
    {
        private readonly ILogger<MessageController> _logger;
        //private readonly BaseDbContext _context;
        private readonly ITokenService _tokenService;
        private readonly EmailService _emailService;
        private readonly IMessageRepository _messageRepo;
        private readonly IClientRepository _clientRepo;
        private readonly IDoctorRepository _doctorRepo;

        public MessageController(
            ILogger<MessageController> logger,
            BaseDbContext context,
            ITokenService tokenService,
            EmailService emailService,
            IMessageRepository messageRepository,
            IClientRepository clientRepository,
            IDoctorRepository doctorRepository
            )
        {
            _logger = logger;
            _tokenService = tokenService;
            //_context = context;
            _emailService = emailService;
            _messageRepo = messageRepository;
            _clientRepo = clientRepository;
            _doctorRepo = doctorRepository;
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
        [Route("doctor/message/send")]
        [HttpPost]
        public async Task<ActionResult<MessageDto>> SendDoctorMessagesById(SendMessageDto sendDto)
        {
            string doctorSealNumber = _tokenService.ReadToken(HttpContext.Request.Headers["Authorization"]);
            //TODO validáció
            Message message = new Message();
            message.Sender = await _doctorRepo.GetDoctorBySealNumberAsync(doctorSealNumber);
            message.Reciever = await _clientRepo.GetClientByMedNumberAsync(sendDto.RecieverNumber);
            message.Content = sendDto.Content;
            message.SenderDeleted = false;
            message.RecieverDeleted = false;

            _messageRepo.AddMessage(message);
            await _messageRepo.SaveAllAsync();

            return new MessageDto(message);
        }

        [Authorize]
        [Route("doctor/messages/{medNumber}")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetDoctorMessages(string medNumber)
        {
            string doctorSealNumber = _tokenService.ReadToken(HttpContext.Request.Headers["Authorization"]);
            Client client = await _clientRepo.GetClientByMedNumberAsync(medNumber);
            Doctor doctor = await _doctorRepo.GetDoctorBySealNumberAsync(doctorSealNumber);
            List<Message> messages = (await _messageRepo.GetDoctorMessagesWithClientAsync(doctor, client));

            List<Message> unreadMessages = await _messageRepo.GetUnreadRecievedMessages(doctor);
            if (unreadMessages.Any())
            {
                foreach (var unreadMessage in unreadMessages)
                {
                    unreadMessage.DateRead = DateTime.Now;
                }
            }
            await _messageRepo.SaveAllAsync();
            List<MessageDto> messageDtos = new List<MessageDto>();
            messages.ForEach(x => messageDtos.Add(new MessageDto(x)));

            return messageDtos;
        }

        [Authorize]
        [Route("client/message/send")]
        [HttpPost]
        public async Task<ActionResult<IEnumerable<MessageDto>>> SendClientMessage()
        {

            return null;
        }

        [Authorize]
        [Route("client/messages")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetClientMessages()
        {

            return null;
        }

        [Authorize]
        [Route("user/message/delete/{messageId}")]
        [HttpDelete]
        public async Task<ActionResult<IEnumerable<MessageDto>>> DeleteMessageById()
        {

            return null;
        }
    }
}
