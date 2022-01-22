using System;
using System.Collections.Generic;
using System.Text;

namespace ServiciosGenericos.Peticion
{
   public class ClasePeticion<T>
    {
        public ClasePeticion()
        {

        }
        public T Clase { get; set; }
        public bool Transaccionalidad { get; set; }
        public string Token { get; set; }

    }
}
