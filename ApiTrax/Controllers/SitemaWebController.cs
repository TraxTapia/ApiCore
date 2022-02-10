using Core.Implement.Trax;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Models.Api;
using Models.Models.Request;
using Models.Models.Response;
using Models.Settings;
using Negocio.Trax;
using ServiciosGenericos.Peticion;
using ServiciosGenericos.Respuesta;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiTrax.Controllers
{
    [Route("[controller]")]
    [ApiController]

    public class SitemaWebController : Controller
    {
        ImplementServices implement = null;
        private readonly ILogger logger;
        private readonly NegocioSistemaWeb negocio;
        public SitemaWebController(IOptions<AppSettings> _appSettings, ILogger<SitemaWebController> logger)
        {
            implement = new ImplementServices(_appSettings);
            this.logger = logger;
        }
        [Route("api/Info")]
        [HttpGet]
        public Task<RespuestaSimple> Info()
        {
            return implement.Info();
        }
        [HttpPost]
        [Route("api/Tkn")]
        public Task<RespuestaSimple> GenerarToken(ClasePeticion<RequestTknUsuario> request)
        {
            return implement.GenerarToken(request);
        }

        [HttpPost]
        [Route("api/AgregarUsuario")]
        //[Authorize]
        public Task<RespuestaSimple> AgregarUsuario(ClasePeticion<RequestUsuario> request)
        {
            return implement.AgregarUsuario(request);
        }
        [HttpPost]
        [Route("Api/LoginUsuario")]
        public Task<RespuestaSimple> LoginUsuario(ClasePeticion<RequestUsuario> request)
        {
            return implement.LoginUsuario(request);
        }
        [HttpGet]
        [Route("Api/ObtenerUsuarios")]
        public Task<RespuestaData<List<ListaUsuariosResponse>>> ObtenerListaUsuarios()
        {
            return implement.ObtenerUsuarios();
        }

    }
}
