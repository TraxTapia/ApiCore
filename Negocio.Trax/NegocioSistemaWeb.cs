using AutoMapper;
using Data.DAO.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Models;
using Models.Api;
using Models.Enum;
using Models.Models.MensajeriVM;
using Models.Models.Request;
using Models.Models.Response;
using Models.Models.SistemaWeb;
using Models.Settings;
using Models.SistemaWebCtxDB;
using Negocio.Trax.ApiNegocio;
using ServiciosGenericos.Respuesta;

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Utilidades;
using Seguridad = Utilidades;

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

        #region Autenticacion

        public RespuestaSimple GenerarToken(RequestTknUsuario request)
        {
            RespuestaSimple response = new RespuestaSimple();
            RequestTknUsuario parametrosToken = new RequestTknUsuario()
            {
                Usuario = request.Usuario,
                Password = request.Password,
            };
            try
            {

                var tokenResponse = token(parametrosToken);
                if (tokenResponse != null)
                {
                    response.result = 200;
                    response.mensaje = tokenResponse;
                }
                else
                {
                    response.result = 500;
                    response.mensaje = "Ocurrio un error.";

                }

            }
            catch (Exception)
            {

                throw;
            }
            return response;

        }

        #endregion

        #region SISTEMAWEB
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
                NegocioMensajeria requestMail = new NegocioMensajeria();

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
                requestMail.GetSettings(appSettings, "MACPlatillaGeneral.html");
                string accion = "<a href='#'style='color: #F9FAE5; text-decoration: none; font-size: 16px; -webkit-border-radius: 5px; -moz-border-radius: 5px; border-radius: 5px; display: block; padding: 1px 8px;'>REGISTRARSE</a>";

                VMMail mail = new VMMail()
                {
                    to = request.Correo,
                    asunto = "NUEVO USUARIO BIENVENIDO " + String.Join(" ", request.Nombre),
                    mensaje = "Estimado Usuario :" + string.Join(" ", request.Nombre, request.ApellidoPaterno) + ", <br> con correo: " + request.Correo + "",
                    asuntoDetalle = "Bienvenido: "+ string.Join(" ", request.Nombre, request.ApellidoPaterno)+ " a nuestro SISTEMA WEB." ,
                    accion = accion,
                    ListaArchivosB64 = new List<ArchivoBase64>()
                };
                Respuesta responsemail = requestMail.SendMail(mail.to, mail.asunto, mail.mensaje, new List<string>(), string.Empty, mail.asuntoDetalle, mail.accion);


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

                NegocioToken negocio = new NegocioToken(UserId, appSettings);
                RequestTknUsuario requestUser = new RequestTknUsuario()
                {
                    Usuario = request.Correo,
                    Password = request.Contrasena
                };
                var passwordEncriptada = Encriptacion.encriptarPasswordGetHas256(request.Contrasena);
                queryParameters.where = x => x.Correo == request.Correo && x.Contrasena == passwordEncriptada;
                var data = repo.EncontrarPor(queryParameters).FirstOrDefault();
                if (data != null)
                {
                    //var obj = BuildToken(request.Correo);
                    var tokenJwt = negocio.GenerarToken(requestUser);
                    response.result = 200;
                    response.mensaje = tokenJwt.mensaje.ToString();

                }
                else
                {
                    response.result = 0;
                    response.mensaje = "Autenticación fallida el usuario o password son incorrectos";

                }
                repo.Dispose();

            }
            catch (Exception ex)
            {

                response.result = 500;
                response.mensaje = "Ocurrio un error " + ex.Message;
            }
            return response;
        }

        public RespuestaData<List<ListaUsuariosResponse>> ObtenerUsuarios()
        {
            RespuestaData<List<ListaUsuariosResponse>> respuesta = new RespuestaData<List<ListaUsuariosResponse>>
            {
                Datos = new List<ListaUsuariosResponse>(),
                Respuesta = new RespuestaSimple()
            };
            try
            {
                var _usuarios = new List<Usuarios>();
                SistemaWebDAO _DAO = new SistemaWebDAO(UserId, appSettings.Value.ConnectionStrings["cnnSistemaWeb"], "");
                Data.DAO.EF.DAOCRUDGenerico<Usuarios> repo = _DAO.DAOUsuarios(UserId);
                using (var context = new SistemaWebdbcontext(_DAO.ConParams))
                {
                    _usuarios = context.Usuarios.Include(r => r.Rol).Where(u => u.Activo.Equals(true)).ToList();

                }
                if (!_usuarios.Any())
                {
                    respuesta.Respuesta.result = 200;
                    respuesta.Respuesta.mensaje = "No se encontraron resultados";
                    return respuesta;
                }
                respuesta.Datos = (from usr in _usuarios
                                   select new ListaUsuariosResponse
                                   {
                                     Id = usr.Id,
                                     NombreCompleto = string.Join(" ",usr.Nombre,usr.ApellidoPaterno,usr.ApellidoMaterno),
                                     Correo = usr.Correo,
                                     DescripcionRol = usr.Rol.Descripcion,
                                     Activo = usr.Activo,  

                                   }).ToList();
                respuesta.Respuesta.result = 200;
                respuesta.Respuesta.mensaje = "Se obtuvieron los datos con exito";

            }
            catch (Exception ex)
            {
                respuesta.Respuesta.result = 500;
                respuesta.Respuesta.mensaje = "Ocurrio un error - " + ex.Message;
            }
            return respuesta;
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

        private string BuildToken(string correo)
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
        public string token(RequestTknUsuario request)
        {
            var secretKey = appSettings.Value.identitiKeyJWT["Keyjwt"];
            var audienceToken = appSettings.Value.identitiKeyJWT["JWT_AUDIENCE_TOKEN"];
            var issuerToken = appSettings.Value.identitiKeyJWT["JWT_ISSUER_TOKEN"];
            var expireTime = appSettings.Value.identitiKeyJWT["JWT_EXPIRE_MINUTES"];

            var securityKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(secretKey));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            // create a claimsIdentity 
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.Name, request.Usuario),
                new Claim(ClaimTypes.Authentication, request.Password),
                //new Claim(ClaimTypes.Role, request.Password)
            });
            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var jwtSecurityToken = tokenHandler.CreateJwtSecurityToken(
                audience: audienceToken,
                issuer: issuerToken,
                subject: claimsIdentity,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(expireTime)),
                signingCredentials: signingCredentials);

            var jwtTokenString = tokenHandler.WriteToken(jwtSecurityToken);
            return jwtTokenString;


        }
        #endregion
    }
}
