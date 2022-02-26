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
        private readonly BaseDbContext _context;
        private readonly ITokenService _tokenService;
        private readonly EmailService _emailService;

        public MessageController(ILogger<MessageController> logger, BaseDbContext context, ITokenService tokenService, EmailService emailService)
        {
            _logger = logger;
            _tokenService = tokenService;
            _context = context;
            _emailService = emailService;
        }

        #region FileUploadController
        
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
        
        #endregion

        [Authorize]
        [Route("doctor/message/send")]
        [HttpPost]
        public async Task<ActionResult<MessageDto>> SendDoctorMessagesById(SendMessageDto sendDto)
        {
            string doctorId = _tokenService.ReadToken(HttpContext.Request.Headers["Authorization"]);
            //if(await _context._doctors.Include(x => x.Clients).SingleOrDefaultAsync(x => x.Id == doctorId).)

            Message message = new Message();
            message.Sender = await _context._doctors.Include(x => x.Place.City.County).SingleOrDefaultAsync(x => x.Id == doctorId);
            message.Reciever = await _context._clients.Include(x => x.Place.City.County).SingleOrDefaultAsync(x => x.MedNumber == sendDto.RecieverNumber);
            message.Content = sendDto.Content;
            message.SenderDeleted = false;
            message.RecieverDeleted = false;

            await _context._messages.AddAsync(message);
            await _context.SaveChangesAsync();

            return new MessageDto(message);
        }

        [Authorize]
        [Route("doctor/messages/{medNumber}")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetDoctorMessages(string medNumber)
        {
            string doctorId = _tokenService.ReadToken(HttpContext.Request.Headers["Authorization"]);

            List<Message> messages = await _context._messages
                .Include(x => x.Sender.Place.City.County)
                .Include(x => x.Reciever.Place.City.County)
                .Where(m => m.Sender.Id == doctorId && m.Reciever.MedNumber == medNumber
                                        
                )
                .OrderBy(m => m.DateSent)
                .ToListAsync();

            List<Message> unreadMessages =
                messages.Where(m => m.DateRead == null && m.Reciever.MedNumber == medNumber)
                .ToList();
            if (unreadMessages.Any())
            {
                foreach (var unreadMessage in unreadMessages)
                {
                    unreadMessage.DateRead = DateTime.Now;
                }
            }
            await _context.SaveChangesAsync();
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
