using Core.Contratos.Trax;
using Microsoft.Extensions.Options;
using Models.Models.UsuarioTkn;
using Models.Settings;
using ServiciosGenericos.Respuesta;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Implement.Trax
{
    public  class ImplementServices: CoreServices
    {
        private readonly IOptions<AppSettings> appSettings;

        private String UserId = String.Empty;
        private const int ok = 1;
        private string _cadenaConexion = string.Empty;

        public ImplementServices()
        {

        }
        public ImplementServices(IOptions<AppSettings> _appSettings)
        {
            appSettings = _appSettings;

        }
        public ImplementServices(string connectionString)
        {
            _cadenaConexion = connectionString;
        }
        public Task<RespuestaSimple> Info()
        {
            return Task.FromResult<RespuestaSimple>(new RespuestaSimple() { result = 1, mensaje = "API MAC Desarrollo" });
        }
        public Task<RespuestaSimple> GenerateToken(User user)
        {
            try
            {

                return Task.FromResult<RespuestaSimple>(new NegocioAutenticacion(appSettings).Autenticar(user.user, user.password));
            }
            catch (Exception ex)
            {
                return Task.FromResult<RespuestaSimple>(new RespuestaSimple { result = 0, mensaje = ex.Message });

            }
        }
    }
}
