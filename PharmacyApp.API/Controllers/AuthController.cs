using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmaFlow.DTOs.Auth;
using PharmaFlow.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;


namespace PharmaFlow.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;

        public AuthController(IAuthService service)
        {
            _service = service;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var result = await _service.Register(dto);

            return Ok(new
            {
                success = true,
                message = result
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var token = await _service.Login(dto);

            return Ok(new
            {
                success = true,
                data = token
            });
        }


 

        [Authorize]
        [HttpGet("secure")]
        public IActionResult SecureEndpoint()
        {
            return Ok("Authorized access");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("test-admin")]
        public IActionResult TestAdmin()
        {
            return Ok("Admin authorized");
        }


    }
}