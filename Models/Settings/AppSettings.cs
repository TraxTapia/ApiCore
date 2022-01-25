using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Settings
{
   public class AppSettings
    {
        public Dictionary<string, string> LogService { get; set; }
        public Dictionary<string, string> Logs { get; set; }
        public Dictionary<string, string> ConnectionStrings { get; set; }
        public Dictionary<string, string> MailSettings { get; set; }
        public Dictionary<string, string> Servicios { get; set; }
        public Dictionary<string, string> FolderServer { get; set; }
        public Dictionary<string, string> GenerateToken { get; set; }
        public Dictionary<string, string> FiltroAfiliados { get; set; }
        public Dictionary<string, string> Generic { get; set; }
        public Dictionary<string, string> UrlServicios { get; set; }
        public Dictionary<string, string> identitiKeyJWT { get; set; }
        //public List<LayoutExcelSettings> LayoutSiniestralidadEspecial { get; set; }
    }
}
