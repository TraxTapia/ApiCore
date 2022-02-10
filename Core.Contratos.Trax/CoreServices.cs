using Models.Api;
using Models.Models.Request;
using Models.Models.Response;
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
        Task<RespuestaSimple> GenerarToken(ClasePeticion<RequestTknUsuario> request);
        Task<RespuestaSimple> AgregarUsuario(ClasePeticion<RequestUsuario> request);
        Task<RespuestaSimple> LoginUsuario(ClasePeticion<RequestUsuario> request);
        Task<RespuestaData<List<ListaUsuariosResponse>>> ObtenerUsuarios();
    }
}
