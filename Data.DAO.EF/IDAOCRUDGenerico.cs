using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.EF
{
    public interface IDAOCRUDGenerico<T>
    {
        void Actualizar(T entidad);
        void ActualizarPending(T entidad);
        void ActualizarRango(List<T> entidades);
        void Agregar(T entidad);
        void AgregarPending(T entidad);
        List<T> AgregarRango(List<T> entidades);
        int Contar(Expression<Func<T, bool>> where);
        void Detach(T entidad);
        void Dispose();
        void Eliminar(QueryParameters<T> parametrosQuery);
        void EliminarRango(List<T> entidades);
        IEnumerable<T> EncontrarPor(QueryParameters<T> parametrosQuery);
        IEnumerable<T> EncontrarPor(QueryParameters<T> parametrosQuery, ref int totalRows);
        void Log(string userName);
        void Log(int userId);
        bool Save();
    }
}
