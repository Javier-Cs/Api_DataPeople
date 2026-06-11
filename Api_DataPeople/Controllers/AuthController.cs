using Api_DataPeople.DTO;
using Api_DataPeople.Exceptions;
using Api_DataPeople.Services;
using Api_DataPeople.Validacion;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Nottyn.Dtos.salida;
using System.Security.Claims;

namespace Api_DataPeople.Controllers
{
    [ApiController]
    [Route("Api/[controller]")]
    public class AuthController : ControllerBase 
    {
        private readonly JWTService _jwtService;
        private readonly IPeopleDataService _peopleDataService;


        public AuthController(JWTService jwtService, IPeopleDataService peopleDataService)
        {
            _jwtService = jwtService;
            _peopleDataService = peopleDataService;
        }

        [Authorize]
        [HttpGet("me")]
        public IActionResult Me()
        {
            return Ok(new
            {
                IdUsuario = User.FindFirst(
                    ClaimTypes.NameIdentifier)?.Value,

                Nombre = User.FindFirst(
                    ClaimTypes.Name)?.Value,

                Rol = User.FindFirst(
                    ClaimTypes.Role)?.Value,

                //Email = User.FindFirst(ClaimTypes.Email)?.Value
            });
        }



        [HttpGet("hash")]
        public IActionResult GenerarHash(string password)
        {
            return Ok(new
            {
                Hash = BCrypt.Net.BCrypt.HashPassword(password)
            });
        }



        // log out
        [HttpPost("logOut")]
        public IActionResult LogOut() {
            Response.Cookies.Delete(
                "access_token",
                new CookieOptions
                {
                    Secure = true,
                    SameSite = SameSiteMode.None
                }
            );

            return Ok(new
            {
                message="Sesión Cerrada."
            });
        }



        [EnableRateLimiting("login")]
        [HttpPost("login")]
        public async Task<ActionResult<SLUsuarioLoginDto>> LoginUser([FromBody]LoginDto loginDto, CancellationToken ct) {
            try {
                if (string.IsNullOrWhiteSpace(loginDto.Email)) {
                    return BadRequest("Email Requerido.");
                }

                if (string.IsNullOrWhiteSpace(loginDto.Password))
                {
                    return BadRequest("La contraseña es requerida.");
                }
                string? ip = HttpContext.Connection.RemoteIpAddress?.ToString();

                var usuario = await _peopleDataService.LoginAsync(loginDto, ip ?? "IP_NO_DISPONIBLE", ct);
                
                if (usuario == null) {
                    return StatusCode(500, "Error al crear usuario");
                }

                Response.Cookies.Append(
                    "access_token",
                    usuario.Token,
                    new CookieOptions
                    { 
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.None,
                        Expires = usuario.Expiracion
                    }
                );
                
                return Ok(new
                {
                    usuario.idUsuario,
                    usuario.Nombre,
                    usuario.rol,
                });
            }
            catch (LoginException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message,
                    intentosRestantes = ex.IntentosRestantes,
                    bloqueado = ex.Bloqueado
                });
            }

        }
    }
}
