using DoctorSystem.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DoctorSystem.Interfaces
{
    public interface IAccountController
    {
        [HttpPost, Route("client/login")]
        Task<ActionResult<ClientDto>> ClientLogin(ClientLoginDto loginDto);
        [HttpGet, Route("client/delete/{token}")]
        Task<ActionResult> DeleteClient(string token);
        [HttpPut, Route("doctor/login")]
        Task<ActionResult<DoctorDto>> DoctorLogin(DoctorLoginDto loginDto);
        [HttpPut, Route("lost-password")]
        Task<ActionResult> LostPassword(LostPasswordDto lostDto);
        [HttpPost, Route("new-password")]
        Task<ActionResult> NewPassword(NewPassworDto newDto);
        [HttpPost, Route("client/register")]
        Task<ActionResult> Register(RegisterDto registerDto);
        [HttpGet, Route("validate-email/{token}")]
        Task<ActionResult> ValidateEmail(string token);
    }
}
