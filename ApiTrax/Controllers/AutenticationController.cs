using ApiTrax.Auth;
using Core.Implement.Trax;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Models.Models.UsuarioTkn;
using Models.Settings;
using ServiciosGenericos.Respuesta;
using System;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Models.Models.Request;
using System.Text;

namespace ApiTrax.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AutenticationController : ControllerBase
    {
        private readonly ILogger<AutenticationController> _logger;
        private readonly IJwtAuthenticationService _authService;
        private readonly IOptions<AppSettings> appSettings;

        ImplementServices implement = null;
        public AutenticationController(IOptions<AppSettings> _appSettings, ILogger<AutenticationController> logger, IJwtAuthenticationService authService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _authService = authService;
            implement = new ImplementServices(_appSettings);
        }

        //[Route("api/Token")]
        //[HttpPost]
        //public Task<RespuestaSimple> GenerateToken(User user)
        //{
        //    return implement.GenerateToken(user);
        //}
        private IActionResult BuildToken(RequestUsuario request)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.UniqueName,request.Correo),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.Value.identitiKeyJWT["KeySecreta"]));
            var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddHours(1);

            JwtSecurityToken token = new JwtSecurityToken
                (
                issuer: "",
                audience: "",
                claims: claims,
                expires: expiration,
                signingCredentials: credential);

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = expiration,
            });
        }
    }
}
