using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Models.Api;
using Models.Settings;
using ServiciosGenericos.Respuesta;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

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
            try
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
            catch (Exception ex)
            {
                response.result = 500;
                response.mensaje = "Ocurrio un error " + ex.Message;


            }
          
            return response;


        }
    }
}
