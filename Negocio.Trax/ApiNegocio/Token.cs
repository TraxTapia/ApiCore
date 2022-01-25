using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Models.Api;
using Models.Settings;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Negocio.Trax.ApiNegocio
{
    public  class Token
    {
        private  readonly IOptions<AppSettings> appSettings;
        private  String UserId = String.Empty;
        private  string usuario = string.Empty;
        private   string appid = string.Empty;
        private  int minutosLogeo = 0;
        //private SAIDAO daoSAI = null;
        //private MaedicusDAO daoMaedicus = null;
        private  readonly Microsoft.Extensions.Logging.ILogger logger;
        //Crear Mapeos
        //IMapper mapper = new Mapeado().config.CreateMapper();
        public Token()
        {

        }

        public Token(String pusuario)
        {
            UserId = pusuario;

        }
        public Token(String pusuario, IOptions<AppSettings> _appSettings)
        {
            appSettings = _appSettings;
            UserId = pusuario;
            //usuario = appSettings.Value.LogService["usr"];
            appid = appSettings.Value.LogService["appId"];
            minutosLogeo = Int32.Parse(appSettings.Value.LogService["delay"]);
        }

       
    }
}
