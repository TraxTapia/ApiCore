﻿using Core.Implement.Trax;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Models.Models.Request;
using Models.Settings;
using Negocio.Trax;
using ServiciosGenericos.Peticion;
using ServiciosGenericos.Respuesta;
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
        [Route("api/AgregarUsuario")]
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
    }
}
