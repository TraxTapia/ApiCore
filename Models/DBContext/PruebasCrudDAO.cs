using System;
using System.Collections.Generic;
using System.Text;

namespace Models.DBContext
{
    public class PruebasCrudDAO
    {
        private String constring = String.Empty;
        private String rutaLogs = String.Empty;
        String UserId = String.Empty;
        public String ConParams { get => constring; set => constring = value; }
        public PruebasCrudDAO()
        {

        }
        public PruebasCrudDAO(String _UserId, String _pConstring, String _prutaLogs)
        {
            UserId = _UserId;
            constring = _pConstring;
            rutaLogs = _prutaLogs;
        }

        private PruebasCrudContext GetContext()
        {
            return new PruebasCrudContext(constring);
        }

        public void Add<T>(T Register) where T : class
        {
            using (PruebasCrudContext pruebasCrud = new PruebasCrudContext())
            {
                pruebasCrud.Set<T>().Add(Register);
                pruebasCrud.SaveChanges();

            }
        }

        public void Update<T>(T Register) where T : class
        {
            using (PruebasCrudContext pruebasCrud = new PruebasCrudContext())
            {
                pruebasCrud.Entry(Register).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                pruebasCrud.SaveChanges();
            }
        }
        public void Remove<T>(T Register) where T : class
        {
            try
            {
                using (PruebasCrudContext pruebasCrud = new PruebasCrudContext())
                {
                    pruebasCrud.Set<T>().Remove(Register);
                    pruebasCrud.SaveChanges();
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}
