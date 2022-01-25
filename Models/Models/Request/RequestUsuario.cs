using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Models.Request
{
    public class RequestUsuario
    {
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string Correo { get; set; }
        public string Contrasena { get; set; }
        public int IdRol { get; set; }
    }
}
