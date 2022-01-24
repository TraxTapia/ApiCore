using Models.Models.UsuarioTkn;
using ServiciosGenericos.Peticion;
using ServiciosGenericos.Respuesta;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Contratos.Trax
{
   public interface  CoreServices
    {
        Task<RespuestaSimple> Info();
        //Task<RespuestaSimple> AgregarUsuario(ClasePeticion<User> request);
    }
}
