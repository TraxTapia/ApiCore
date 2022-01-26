using Data.DAO.EF;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Models.Api;
using Models.Models.SistemaWeb;
using Models.Settings;
using Models.SistemaWebCtxDB;
using ServiciosGenericos.Respuesta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Utilidades;

namespace Negocio.Trax
{
    public class NegocioToken
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
        public NegocioToken()
        {

        }

        public NegocioToken(String pusuario)
        {
            UserId = pusuario;

        }
        public NegocioToken(String pusuario, IOptions<AppSettings> _appSettings)
        {
            appSettings = _appSettings;
            UserId = pusuario;
            //usuario = appSettings.Value.LogService["usr"];
            appid = appSettings.Value.LogService["appId"];
            minutosLogeo = Int32.Parse(appSettings.Value.LogService["delay"]);
        }
        public RespuestaSimple GenerarToken(RequestTknUsuario request)
        {
            RespuestaSimple response = new RespuestaSimple();
            var secretKey = appSettings.Value.identitiKeyJWT["Keyjwt"];
            var audienceToken = appSettings.Value.identitiKeyJWT["JWT_AUDIENCE_TOKEN"];
            var issuerToken = appSettings.Value.identitiKeyJWT["JWT_ISSUER_TOKEN"];
            var expireTime = appSettings.Value.identitiKeyJWT["JWT_EXPIRE_MINUTES"];
            SistemaWebDAO _DAO = new SistemaWebDAO(UserId, appSettings.Value.ConnectionStrings["cnnSistemaWeb"], "");
            Data.DAO.EF.DAOCRUDGenerico<Usuarios> repo = _DAO.DAOUsuarios(UserId);
            QueryParameters<Usuarios> queryParameters = new QueryParameters<Usuarios>();
            try
            {
                var passwordEncriptada = Encriptacion.encriptarPasswordGetHas256(request.Password);
                queryParameters.where = x => x.Correo == request.Usuario && x.Contrasena == passwordEncriptada;
                var data = repo.EncontrarPor(queryParameters).FirstOrDefault();
                if (data != null)
                {
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
                    response.result = 200;
                    response.mensaje = jwtTokenString;
                }
                else
                {
                    response.result = 500;
                    response.mensaje = "Usuario o contraseña incorrectas o no existen";
                }


            }
            catch (Exception ex)
            {
                response.result = 500;
                response.mensaje = "Ocurrio un error " + ex.Message;
            }
            return response;


        }
    }
}
