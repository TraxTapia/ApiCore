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

namespace ApiTrax.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AutenticationController : ControllerBase
    {
        private readonly ILogger<AutenticationController> _logger;
        private readonly IJwtAuthenticationService _authService;

        ImplementServices implement = null;
        public AutenticationController(IOptions<AppSettings> _appSettings, ILogger<AutenticationController> logger, IJwtAuthenticationService authService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _authService = authService;
            implement = new ImplementServices(_appSettings);
        }

        [Route("api/Token")]
        [HttpPost]
        public Task<RespuestaSimple> GenerateToken(User user)
        {
            return implement.GenerateToken(user);
        }
    }
}
