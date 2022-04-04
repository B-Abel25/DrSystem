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
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DoctorSystem.Controllers
{
    [ApiController]
    [Route("private/")]
    public class MessageController : ControllerBase
    {
        private readonly ILogger<MessageController> _logger;
        private readonly ITokenService _tokenService;
        private readonly EmailService _emailService;
        private readonly IMessageRepository _messageRepo;
        private readonly IClientRepository _clientRepo;
        private readonly IDoctorRepository _doctorRepo;
        private readonly Regex sWhitespace = new Regex(@"\s+");
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

            _messageRepo.UpdateMessage(message);
            await _messageRepo.SaveAllAsync();

            return new MessageDto(message);
        }

        [Authorize]
        [Route("doctor/messages/{medNumber}")]
        [HttpGet]
        public async Task<ActionResult<List<MessageDto>>> GetDoctorMessagesWithClient(string medNumber)
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
        [Route("doctor/messages")]
        [HttpGet]
        public async Task<ActionResult<List<MessageDto>>> GetDoctorMessages()
        {
            string doctorSealNumber = _tokenService.ReadToken(HttpContext.Request.Headers["Authorization"]);
            Doctor doctor = await _doctorRepo.GetDoctorBySealNumberAsync(doctorSealNumber);
            List<Message> messages = (await _messageRepo.GetDoctorMessagesAsync(doctor));

            await _messageRepo.SaveAllAsync();
            List<MessageDto> messageDtos = new List<MessageDto>();
            messages.ForEach(x => messageDtos.Add(new MessageDto(x)));

            return messageDtos;
        }

        [Authorize]
        [Route("client/message/send")]
        [HttpPost]
        public async Task<ActionResult<MessageDto>> SendClientMessage(SendMessageDto sendDto)
        {
            if (RemoveWhitespace(sendDto.Content) == "")
            {
                return Unauthorized("Nem lehet üres üzenetet küldeni");
            }

            string clientMedNumber = _tokenService.ReadToken(HttpContext.Request.Headers["Authorization"]);
            //TODO validáció
            Client client = await _clientRepo.GetClientByMedNumberAsync(clientMedNumber);
            Message message = new Message();
            message.Sender = client;
            message.Reciever = client.Doctor;
            message.Content = sendDto.Content;

            _messageRepo.UpdateMessage(message);
            await _messageRepo.SaveAllAsync();

            return new MessageDto(message);
        }

        [Authorize]
        [Route("client/messages")]
        [HttpGet]
        public async Task<ActionResult<List<MessageDto>>> GetClientMessages()
        {
            string clientMedNumber = _tokenService.ReadToken(HttpContext.Request.Headers["Authorization"]);
            Client client = await _clientRepo.GetClientByMedNumberAsync(clientMedNumber);
            List<Message> messages = (await _messageRepo.GetClientMessagesAsync(client));

            List<Message> unreadMessages = await _messageRepo.GetUnreadRecievedMessages(client);
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
        
        #region DeleteMessageEndpoints
        /*
        [Authorize]
        [Route("client/message/delete")]
        [HttpDelete]
        public async Task<ActionResult<MessageDto>> ClientMessageDelete(DeleteMessageDto deleteDto)
        {
            string clientMedNumber = _tokenService.ReadToken(HttpContext.Request.Headers["Authorization"]);
            Client client = await _clientRepo.GetClientByMedNumberAsync(clientMedNumber);
            Message m = await _messageRepo.GetMessageByContentAndDateSentAndUserId(deleteDto.Content,deleteDto.DateSent,client);
            if (m.Sender.Id == client.Id)
            {
                m.SenderDeleted = true;
            }
            else
            {
                m.RecieverDeleted = true;
            }
            _messageRepo.UpdateMessage(m);
            await _messageRepo.SaveAllAsync();
            return Accepted();
        }
       
        [Authorize]
        [Route("doctor/message/delete")]
        [HttpDelete]
        public async Task<ActionResult<MessageDto>> DoctorMessageDelete(DeleteMessageDto deleteDto)
        {            
            string doctorSealNumber = _tokenService.ReadToken(HttpContext.Request.Headers["Authorization"]);
            Doctor doctor = await _doctorRepo.GetDoctorBySealNumberAsync(doctorSealNumber);
            Message m = await _messageRepo.GetMessageByContentAndDateSentAndUserId(deleteDto.Content, deleteDto.DateSent, doctor);
            if (m.Sender.Id == doctor.Id)
            {
                m.SenderDeleted = true;
            }
            else
            {
                m.RecieverDeleted = true;
            }

            _messageRepo.UpdateMessage(m);
            await _messageRepo.SaveAllAsync();
            return Accepted();
        }
        */
        #endregion
        
        [Authorize]
        [Route("doctor/messages/unread")]
        [HttpGet]
        public async Task<ActionResult<List<ClientDto>>> GetDoctorUnreadMessages()
        {
            string doctorSealNumber =  _tokenService.ReadToken(HttpContext.Request.Headers["Authorization"]);
            Doctor doctor = await _doctorRepo.GetDoctorBySealNumberAsync(doctorSealNumber);

            List<ClientDto> unreadSenders = new List<ClientDto>();
            List<User> users = (await _messageRepo.GetUnreadRecievedMessages(doctor))
                .Select(x => x.Sender)
                .Distinct()
                .ToList();

            foreach (var user in users)
            {
                Client c = await _clientRepo.GetClientByIdAsync(user.Id);
                unreadSenders.Add(new ClientDto(c));
            }
            return unreadSenders;
        }

        private string RemoveWhitespace(string input)
        {
            return sWhitespace.Replace(input, "");
        }

        [Authorize]
        [Route("doctor/send-email")]
        [HttpPost]
        public async Task<ActionResult> SendEmailEveryBody(EmailDto dto)
        {
            string doctorSealNumber = _tokenService.ReadToken(HttpContext.Request.Headers["Authorization"]);
           
            Doctor doctor = await _doctorRepo.GetDoctorBySealNumberAsync(doctorSealNumber);

            _emailService.SendEmailToEverybody(doctor, dto.Subject, dto.Content);

            return Accepted();
        }
    }
}
