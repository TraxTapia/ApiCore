using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Data.DAO.EF
{
    public class DAOCRUDGenerico<T> :IDisposable, IDAOCRUDGenerico<T> where T : class, new ()
    {
        private string usuario = "";
        private DbContext dbg;
        private bool disposed;
        private bool haycambios;

        public DbContext context
        {
            get => this.dbg;
            set => this.dbg = value;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize((object)this);
        }

        protected virtual void Dispose(bool _disposing)
        {
            if (this.disposed)
                return;
            if (_disposing && this.dbg != null)
                this.dbg.Dispose();
            this.disposed = true;
        }

        public DAOCRUDGenerico()
        {
        }

        public DAOCRUDGenerico(DbContext pDbContext) => this.dbg = pDbContext;

        public DAOCRUDGenerico(string _userName, DbContext pDbContext)
        {
            this.usuario = _userName;
            this.dbg = pDbContext;
        }

        public void Agregar(T _entidad)
        {
            using (this.dbg)
            {
                this.dbg.Entry<T>(_entidad).State = EntityState.Added;
                this.dbg.SaveChanges();
                this.haycambios = false;
            }
        }

        public List<T> AgregarRango(List<T> _entidades)
        {
            using (this.dbg)
            {
                this.dbg.AddRange((IEnumerable<object>)_entidades);
                this.dbg.SaveChanges();
                this.haycambios = false;
            }
            return _entidades;
        }

        public T AgregarIdentity(T _entidad)
        {
            using (this.dbg)
            {
                this.dbg.Entry<T>(_entidad).State = EntityState.Added;
                this.dbg.SaveChanges();
                this.haycambios = false;
            }
            return _entidad;
        }

        public void AgregarPending(T _entidad)
        {
            this.haycambios = true;
            this.dbg.Entry<T>(_entidad).State = EntityState.Added;
        }

        public void Actualizar(T _entidad)
        {
            using (this.dbg)
            {
                this.dbg.Entry<T>(_entidad).State = EntityState.Modified;
                this.dbg.SaveChanges();
                this.haycambios = false;
            }
        }

        public void ActualizarPending(T _entidad)
        {
            this.haycambios = true;
            this.dbg.Entry<T>(_entidad).State = EntityState.Modified;
        }

        public bool Save()
        {
            try
            {
                if (this.haycambios)
                    this.dbg.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Contar(Expression<Func<T, bool>> _where) => this.dbg.Set<T>().Where<T>(_where).Count<T>();

        public IEnumerable<T> EncontrarPorOrdenDefault(QueryParameters<T> _parametrosQuery)
        {
            Expression<Func<T, bool>> expression = (Expression<Func<T, bool>>)(x => true);
            Expression<Func<T, bool>> predicate = _parametrosQuery.where == null ? expression : _parametrosQuery.where;
            return _parametrosQuery.count == 0 ? (IEnumerable<T>)this.dbg.Set<T>().Where<T>(predicate).ToList<T>() : (IEnumerable<T>)this.dbg.Set<T>().Where<T>(predicate).Skip<T>((_parametrosQuery.page - 1) * _parametrosQuery.count).Take<T>(_parametrosQuery.count).ToList<T>();
        }

        public IEnumerable<T> EncontrarPor(QueryParameters<T> _parametrosQuery)
        {
            DAOCRUDGenerico<T>.OrderByClass orderByClass = this.ObtenerOrderBy(_parametrosQuery);
            Expression<Func<T, bool>> expression = (Expression<Func<T, bool>>)(x => true);
            Expression<Func<T, bool>> predicate = _parametrosQuery.where == null ? expression : _parametrosQuery.where;
            if (orderByClass.IsAcending)
            {
                if (_parametrosQuery.count != 0)
                    return (IEnumerable<T>)this.dbg.Set<T>().Where<T>(predicate).OrderBy<T, object>(orderByClass.OrderBy).Skip<T>((_parametrosQuery.page - 1) * _parametrosQuery.count).Take<T>(_parametrosQuery.count).ToList<T>();
                return orderByClass.OrderBy != null ? (IEnumerable<T>)this.dbg.Set<T>().Where<T>(predicate).OrderBy<T, object>(orderByClass.OrderBy).ToList<T>() : (IEnumerable<T>)this.dbg.Set<T>().Where<T>(predicate).ToList<T>();
            }
            if (_parametrosQuery.count != 0)
                return (IEnumerable<T>)this.dbg.Set<T>().Where<T>(predicate).OrderByDescending<T, object>(orderByClass.OrderBy).Skip<T>((_parametrosQuery.page - 1) * _parametrosQuery.count).Take<T>(_parametrosQuery.count).ToList<T>();
            return orderByClass.OrderBy != null ? (IEnumerable<T>)this.dbg.Set<T>().Where<T>(predicate).OrderByDescending<T, object>(orderByClass.OrderBy).ToList<T>() : (IEnumerable<T>)this.dbg.Set<T>().Where<T>(predicate).ToList<T>();
        }

        public IEnumerable<T> EncontrarPor(
          QueryParameters<T> _parametrosQuery,
          ref int _totalRows)
        {
            DAOCRUDGenerico<T>.OrderByClass orderByClass = this.ObtenerOrderBy(_parametrosQuery);
            Expression<Func<T, bool>> expression = (Expression<Func<T, bool>>)(x => true);
            Expression<Func<T, bool>> predicate = _parametrosQuery.where == null ? expression : _parametrosQuery.where;
            _totalRows = this.dbg.Set<T>().Where<T>(predicate).Count<T>();
            return orderByClass.IsAcending ? (IEnumerable<T>)this.dbg.Set<T>().Where<T>(predicate).OrderBy<T, object>(orderByClass.OrderBy).Skip<T>((_parametrosQuery.page - 1) * _parametrosQuery.count).Take<T>(_parametrosQuery.count).ToList<T>() : (IEnumerable<T>)this.dbg.Set<T>().Where<T>(predicate).OrderByDescending<T, object>(orderByClass.OrderBy).Skip<T>((_parametrosQuery.page - 1) * _parametrosQuery.count).Take<T>(_parametrosQuery.count).ToList<T>();
        }

        private DAOCRUDGenerico<T>.OrderByClass ObtenerOrderBy(
          QueryParameters<T> _parametrosQuery)
        {
            return _parametrosQuery.orderBy == null ? new DAOCRUDGenerico<T>.OrderByClass(_parametrosQuery.orderByDesc, false) : new DAOCRUDGenerico<T>.OrderByClass(_parametrosQuery.orderBy, true);
        }

        public void Log(string _userName) => this.usuario = _userName;

        public void Log(int _userId) => throw new NotImplementedException();

        public void Eliminar(QueryParameters<T> parametrosQuery)
        {
            using (this.dbg)
            {
                IEnumerable<T> entities = this.EncontrarPor(parametrosQuery);
                this.dbg.Set<T>().RemoveRange(entities);
                this.dbg.SaveChanges();
            }
        }

        public void Detach(T entidad) => this.dbg.Entry<T>(entidad).State = EntityState.Detached;

        public void EliminarRango(List<T> entidades)
        {
            using (this.dbg)
            {
                this.dbg.RemoveRange((IEnumerable<object>)entidades);
                this.dbg.SaveChanges();
                this.haycambios = false;
            }
        }

        public void ActualizarRango(List<T> entidades)
        {
            using (this.dbg)
            {
                this.dbg.UpdateRange((IEnumerable<object>)entidades);
                this.dbg.SaveChanges();
                this.haycambios = false;
            }
        }

        private class OrderByClass
        {
            public OrderByClass()
            {
            }

            public OrderByClass(Func<T, object> _orderBy, bool _isAcending)
            {
                this.OrderBy = _orderBy;
                this.IsAcending = _isAcending;
            }

            public Func<T, object> OrderBy { get; set; }

            public bool IsAcending { get; set; }
        }

    }
}
