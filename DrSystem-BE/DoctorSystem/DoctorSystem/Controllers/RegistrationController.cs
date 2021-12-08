using DoctorSystem.Dtos;
using DoctorSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoctorSystem.Controllers
{
    [ApiController]
    [Route("/registration")]
    public class RegistrationController : ControllerBase
    {
        private readonly ILogger<RegistrationController> _logger;
        private readonly IUserService _userService;


        public RegistrationController(ILogger<RegistrationController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;

        }

        [HttpPost]
        public IActionResult PostRoom(UserDto user)
        {
            try
            {
                return Ok(this._userService.PostUser(user));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPut]
        public IActionResult PutRoom(string Id, UserDto user)
        {
            try
            {
                return Ok(this._userService.PutUser(Id, user));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpDelete]
        public IActionResult DeleteRoom(string userId)
        {
            try
            {
                return Ok(this._userService.DeleteUser(userId));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("{userId}")]
        [HttpGet]
        public IActionResult GetRoom(string userId)
        {
            try
            {
                return Ok(this._userService.GetUser(userId));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}