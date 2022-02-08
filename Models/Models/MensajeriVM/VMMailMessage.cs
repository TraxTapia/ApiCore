using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Models.MensajeriVM
{
    public class VMMailMessage
    {
        public VMMailMessage()
        {

        }

        public string to { get; set; }
        public string asunto { get; set; }
        public string mensaje { get; set; }
        public List<string> Files { get; set; }
        public string ruta { get; set; }
        public string asuntoDetalle { get; set; }
        public string accion { get; set; }
        public List<ArchivoBase64> ListaArchivosB64 { get; set; }
    }
}
