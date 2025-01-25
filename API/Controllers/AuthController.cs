using CamWebRtc.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CamWebRtc.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IJwtService _jwtService;
        public AuthController(IJwtService jwtService)
        {
            _jwtService = jwtService;
        }
        // POST api/<AuthController>
        [HttpPost("login")]
        public IActionResult Login(LoginModel login)
        {
            if (login.Username == "admin" && login.Password == "senha123")
            {
                var token = _jwtService.GenerateToken("1", "Admin");
                return Ok(new { Token = token });
            }

            return Unauthorized("Credenciais inválidas");
        }
    }
}
