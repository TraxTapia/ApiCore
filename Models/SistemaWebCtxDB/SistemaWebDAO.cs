using Models.Models.SistemaWeb;
using System;
using System.Collections.Generic;
using System.Text;


namespace Models.SistemaWebCtxDB
{
    public  class SistemaWebDAO
    {
        private String constring = String.Empty;
        private String rutaLogs = String.Empty;
        String UserId = String.Empty;
        public String ConParams { get => constring; set => constring = value; }
        public SistemaWebDAO()
        {

        }
        public SistemaWebDAO(String _UserId, String _pConstring, String _prutaLogs)
        {
            UserId = _UserId;
            constring = _pConstring;
            rutaLogs = _prutaLogs;
        }

        private SistemaWebdbcontext GetContext()
        {
            return new SistemaWebdbcontext(constring);
        }

        public Data.DAO.EF.DAOCRUDGenerico<Usuarios> DAOUsuarios(string userId)
        {
            return new Data.DAO.EF.DAOCRUDGenerico<Usuarios>(userId, GetContext());
        }
        //public 
    }
}
