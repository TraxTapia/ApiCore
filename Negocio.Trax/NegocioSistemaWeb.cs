using AutoMapper;
using Data.DAO.EF;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Models.Enum;
using Models.Models.Request;
using Models.Models.SistemaWeb;
using Models.Settings;
using Models.SistemaWebCtxDB;
using ServiciosGenericos.Respuesta;

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Utilidades;

namespace Negocio.Trax
{
    public class NegocioSistemaWeb
    {
        private readonly IOptions<AppSettings> appSettings;
        private String UserId = String.Empty;
        private string usuario = string.Empty;
        private string appid = string.Empty;
        private int minutosLogeo = 0;
        //private SAIDAO daoSAI = null;
        //private MaedicusDAO daoMaedicus = null;
        private readonly Microsoft.Extensions.Logging.ILogger logger;
        //Crear Mapeos
        //IMapper mapper = new Mapeado().config.CreateMapper();
        public NegocioSistemaWeb()
        {

        }

        public NegocioSistemaWeb(String pusuario)
        {
            UserId = pusuario;

        }
        public NegocioSistemaWeb(String pusuario, IOptions<AppSettings> _appSettings)
        {
            appSettings = _appSettings;
            UserId = pusuario;
            //usuario = appSettings.Value.LogService["usr"];
            appid = appSettings.Value.LogService["appId"];
            minutosLogeo = Int32.Parse(appSettings.Value.LogService["delay"]);
        }
        #region
        public RespuestaSimple AgregarUsuario(RequestUsuario request)
        {
            RespuestaSimple response = new RespuestaSimple();
            bool existe = false;
            try
            {
                SistemaWebDAO _DAO = new SistemaWebDAO(UserId, appSettings.Value.ConnectionStrings["cnnSistemaWeb"], "");
                Data.DAO.EF.DAOCRUDGenerico<Usuarios> repo = _DAO.DAOUsuarios(UserId);

                var passwordEncriptada = Encriptacion.encriptarPasswordGetHas256(request.Contrasena);

                //var encriptarPassword = Encoding.UTF8.GetBytes(NegocioSistemaWeb.GetHash())
                Usuarios addUsuario = new Usuarios()
                {
                    Nombre = request.Nombre.Trim(),
                    ApellidoPaterno = request.ApellidoPaterno.Trim(),
                    ApellidoMaterno = request.ApellidoMaterno.Trim(),
                    Correo = request.Correo.Trim(),
                    Contrasena = passwordEncriptada,
                    IdRol = request.IdRol,
                    FechaCreacion = DateTime.Now,
                    Activo = EstatusGeneral.Activo

                };

                using (SistemaWebdbcontext _context = new SistemaWebdbcontext(_DAO.ConParams))
                {
                    existe = _context.Usuarios.Any(x => x.Correo == addUsuario.Correo);

                }
                if (existe)
                {
                    response.result = 200;
                    response.mensaje = "Ya existe un usuario con el correo " + request.Correo + " ingrese otro correo";
                    return response;
                }
                repo.Agregar(addUsuario);
                response.result = 200;
                response.mensaje = "Usuario agregado exitosamente.";
                repo.Dispose();

            }
            catch (Exception ex)
            {
                response.result = 0;
                response.mensaje = "Ocurrio un error - " + ex.Message;

            }
            return response;

        }
        public RespuestaSimple LoginUsuario(RequestUsuario request)
        {
            RespuestaSimple response = new RespuestaSimple();
            try
            {
                SistemaWebDAO _DAO = new SistemaWebDAO(UserId, appSettings.Value.ConnectionStrings["cnnSistemaWeb"], "");
                Data.DAO.EF.DAOCRUDGenerico<Usuarios> repo = _DAO.DAOUsuarios(UserId);
                QueryParameters<Usuarios> queryParameters = new QueryParameters<Usuarios>();

                var passwordEncriptada = Encriptacion.encriptarPasswordGetHas256(request.Contrasena);

                queryParameters.where = x => x.Correo == request.Correo && x.Contrasena == passwordEncriptada;

                var data = repo.EncontrarPor(queryParameters).FirstOrDefault();

                //RequestUsuario _request = new RequestUsuario()
                //{
                //    Correo = request.Correo,
                //};
                if (data != null)
                {
                   var obj = BuildToken(request.Correo);
                    response.result = 200;
                    response.mensaje = obj.ToString();

                    //response.result = 200;
                    //response.mensaje = "Autenticacion correcta";

                }
                else
                {
                    response.result = 0;
                    response.mensaje = "Autenticación fallida";

                }
                repo.Dispose();

            }
            catch (Exception ex)
            {

                response.result = 200;
                response.mensaje = "Ocurrio un error " + ex.Message;
            }
            return response;
        }
            #endregion


            #region ReferenceToken
            public static String GetHash(String text, String key)
            {
                // change according to your needs, an UTF8Encoding
                // could be more suitable in certain situations
                ASCIIEncoding encoding = new ASCIIEncoding();

                Byte[] textBytes = encoding.GetBytes(text);
                Byte[] keyBytes = encoding.GetBytes(key);

                Byte[] hashBytes;

                using (HMACSHA1 hash = new HMACSHA1(keyBytes))
                    hashBytes = hash.ComputeHash(textBytes);

                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }

        private string BuildToken(string  correo)
        {
            string Tkn = string.Empty;
            string Exiration = string.Empty;
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.UniqueName,correo),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.Value.identitiKeyJWT["Keyjwt"]));
            var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddHours(1);

            JwtSecurityToken token = new JwtSecurityToken
                (
                issuer: "",
                audience: "",
                claims: claims,
                expires: expiration,
                signingCredentials: credential);
            Tkn = new JwtSecurityTokenHandler().WriteToken(token);
            //return new
            //{
            //    token = new JwtSecurityTokenHandler().WriteToken(token),
            //    expiration = expiration,
            //};
            return Tkn;
        }
        #endregion
    }
    }
