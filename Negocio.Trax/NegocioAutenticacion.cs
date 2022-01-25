using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Models.Models.InformacionUsuario;
using Models.Models.Request;
using Models.Settings;
using ServiciosGenericos.Respuesta;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using System.Text;

namespace Negocio.Trax
{
    public class NegocioAutenticacion
    {
        private readonly IOptions<AppSettings> appSettings;
        private string _cadenaConexion = string.Empty;
        private string usuario = string.Empty;
        private string appid = string.Empty;
        private string UsuarioAdmin = string.Empty;
        private string PasswordAdmin = string.Empty;
        private int minutosLogeo = 0;

        public BinaryReader JwrRegisteredClaimNames { get; private set; }

        public NegocioAutenticacion()
        {

        }
        public NegocioAutenticacion(IOptions<AppSettings> _appSettings)
        {
            appSettings = _appSettings;
            usuario = appSettings.Value.LogService["usr"];

            UsuarioAdmin = appSettings.Value.GenerateToken["User"];
            PasswordAdmin = appSettings.Value.GenerateToken["Password"];
            minutosLogeo = Int32.Parse(appSettings.Value.LogService["delay"]);
        }

        public RespuestaSimple GenerateToken(int IdUsuario, string User, string Nombre, string ApellidoPaterno, string ApellidoMaterno)
        {
            RespuestaSimple result = new RespuestaSimple();
            appid = GenerarToken(new InfoUsuario
            {
                idUsuario = IdUsuario,
                Usuario = User,
                Nombre = Nombre,
                ApellidoPaterno = ApellidoPaterno,
                ApellidoMaterno = ApellidoMaterno
            });
            //Byte[] appData = Encriptacion.Encriptar(appid, User);
            //string appidenc = Convert.ToBase64String(appData);
            result.result = 1;
            //result.mensaje = appidenc;
            return result;
        }
        private string GenerarToken(InfoUsuario usr)
        {

            DateTime fechaFin = DateTime.Now.AddMinutes(minutosLogeo);
            //Contiene información de quien inicio la sesion

            StringBuilder stb = new StringBuilder();
            stb.Append(fechaFin.ToString("yyyy-MM-ddTHH:mm"));
            stb.Append("@");
            stb.Append(usr.idUsuario.ToString());
            stb.Append("@");
            stb.Append(usr.Usuario);
            stb.Append("@");
            stb.Append(usr.Nombre).Append(" ").Append(usr.ApellidoPaterno)
               .Append(" ").Append(usr.ApellidoMaterno);
            return stb.ToString();
        }


       
    }
}
