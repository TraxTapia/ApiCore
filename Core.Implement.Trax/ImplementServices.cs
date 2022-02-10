using Core.Contratos.Trax;
using Microsoft.Extensions.Options;
using Models.Api;
using Models.Models.Request;
using Models.Models.Response;
using Models.Models.UsuarioTkn;
using Models.Settings;
using Negocio.Trax;
using ServiciosGenericos.Peticion;
using ServiciosGenericos.Respuesta;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Implement.Trax
{
    public class ImplementServices : CoreServices
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

        public Task<RespuestaSimple> GenerarToken(ClasePeticion<RequestTknUsuario> request)
        {

            RespuestaSimple response = new RespuestaSimple();

            response.result = ok;
            response.mensaje = "AdminCore";
            if (response.result == ok)
            {
                NegocioToken negocio = new NegocioToken(UserId, appSettings);
                try
                {
                    response = negocio.GenerarToken(request.Clase);
                }
                catch (Exception ex)
                {

                    throw new Exception(ex.Message);
                }

            }
            return Task.FromResult<RespuestaSimple>(response);

        }
        public Task<RespuestaSimple> AgregarUsuario(ClasePeticion<RequestUsuario> request)
        {
            //RespuestaSimple response = new NegocioAutenticacion(appSettings);
            RespuestaSimple response = new RespuestaSimple();


            response.result = ok;
            response.mensaje = "AdminCore";
            if (response.result == ok)
            {
                NegocioSistemaWeb negocio = new NegocioSistemaWeb(UserId, appSettings);
                try
                {
                    response = negocio.AgregarUsuario(request.Clase);
                }
                catch (Exception ex)
                {

                    throw new Exception(ex.Message);
                }

            }
            return Task.FromResult<RespuestaSimple>(response);

        }
        public Task<RespuestaSimple> LoginUsuario(ClasePeticion<RequestUsuario> request)
        {
            RespuestaSimple response =new  RespuestaSimple();

            response.result = ok;
            response.mensaje = "AdminCore";
            if (response.result == ok)
            {
                NegocioSistemaWeb negocio = new NegocioSistemaWeb(UserId, appSettings);
                try
                {
                    response = negocio.LoginUsuario(request.Clase);
                }
                catch (Exception ex)
                {

                    throw new Exception(ex.Message);
                }

            }
            return Task.FromResult<RespuestaSimple>(response);
           

        }
        public Task<RespuestaData<List<ListaUsuariosResponse>>> ObtenerUsuarios()
        {
            RespuestaData<List<ListaUsuariosResponse>> respuesta = new RespuestaData<List<ListaUsuariosResponse>>
            {
                Datos = new List<ListaUsuariosResponse>(),
                Respuesta = new RespuestaSimple()
            };

                NegocioSistemaWeb negocio = new NegocioSistemaWeb(UserId, appSettings);
                try
                {
                respuesta = negocio.ObtenerUsuarios();
                }
                catch (Exception ex)
                {

                    throw new Exception(ex.Message);
                }

            
            return Task.FromResult<RespuestaData<List<ListaUsuariosResponse>>>(respuesta);
           

        }
      
     
    }
}
