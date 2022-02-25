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
        public async Task<ActionResult<IEnumerable<MessageDto>>> SendDoctorMessagesById(SendMessageDto sendDto)
        {
            string doctorId = _tokenService.ReadToken(HttpContext.Request.Headers["Authorization"]);
            //if(await _context._doctors.Include(x => x.Clients).SingleOrDefaultAsync(x => x.Id == doctorId).)

            Message message = new Message();
            message.Sender = await _context._doctors.SingleOrDefaultAsync(x => x.Id == doctorId);
            message.Reciever = await _context._clients.SingleOrDefaultAsync(x => x.MedNumber == sendDto.RecieverNumber);
            message.Content = sendDto.Content;
            message.SenderDeleted = false;
            message.RecieverDeleted = false;

            await _context._messages.AddAsync(message);
            await _context.SaveChangesAsync();

            List<Message> messages = await _context._messages.Include(x => x.Sender).Include(x => x.Reciever).ToListAsync();
            List<MessageDto> messageDtos = new List<MessageDto>();
            foreach (var oneMessage in messages)
            {
                if (oneMessage.Sender.Id == doctorId || oneMessage.Reciever.Id == doctorId)
                {
                    messageDtos.Add(new MessageDto(oneMessage));
                }
            }
            return messageDtos;
        }

        [Authorize]
        [Route("doctor/messages")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetDoctorMessages()
        {

           List<Message> messages = await _context._messages.Include(x => x.Sender.Place.City.County).Include(x => x.Reciever.Place.City.County).ToListAsync();
            return null;
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
