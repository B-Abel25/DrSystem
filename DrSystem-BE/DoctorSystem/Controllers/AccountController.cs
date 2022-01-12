using DoctorSystem.Dtos;
using DoctorSystem.Model.Exceptions;
using DoctorSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace DoctorSystem.Controllers
{
    [ApiController]
    [Route("public")]
    public class AccountController : ControllerBase
    {


        private readonly ILogger<AccountController> _logger;
        private readonly AccountService _accountService;


        public AccountController(ILogger<AccountController> logger, AccountService registerService)
        {
            _logger = logger;
            _accountService = registerService;

        }

        [Route("register")]
        [HttpPost]
        public IActionResult Register(RegisterDto registerDto)
        {
            try
            {
                this._accountService.register(registerDto);
                return Accepted();
            }
            catch (AlreadyRegisteredException e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("login")]
        [HttpPost]
        public IActionResult Login(LoginDto loginDto)
        {
            try
            {
                this._accountService.login(loginDto);
            }
            catch (Exception e) when (e is BONotFoundException || e is NotMemberException)

            {
                return BadRequest(e.Message);
            }
            return Ok();

        }
        
        [Route("lost-password")]
        [HttpPost]
        public ActionResult<LostPasswordDto> LostPassword(LostPasswordDto lostPasswordDto)
        {
            try
            {
                return Ok(this._accountService.lostPassword(lostPasswordDto));
            }
            catch (Exception e) when (e is BONotFoundException || e is NotMemberException)

            {
                return BadRequest(e.Message);
            }
        }
    }
}
