using AutoMapper;
using Microsoft.Extensions.Options;
using Models.DBContext;
using Models.Models;
using Models.Settings;
using ServiciosGenericos.Respuesta;
using System;
using System.Collections.Generic;
using System.Text;

namespace Negocio.Trax.NegocioPruebasCrud
{
   public class NegocioPruebas
    {
        private readonly IOptions<AppSettings> appSettings;
        private String UserId = String.Empty;
        private string passwordEncriptada = string.Empty;
        private byte[] key = null;
        private byte[] IV = null;
        private byte[] requestPasswordEncript = null;
        private string encriptadoP = string.Empty;
        private string noEncriptado = string.Empty;
        byte[] enc_bytes = null;
        //Crear Mapeos
        //IMapper mapper = new Mapeado().config.CreateMapper();

        public NegocioPruebas()
        {

        }

        public NegocioPruebas(String pusuario)
        {
            UserId = pusuario;

        }

        public NegocioPruebas(String pusuario, IOptions<AppSettings> _appSettings)
        {
            appSettings = _appSettings;
            UserId = pusuario;
        }
      

        public RespuestaSimple AgregarProducto(Productos request)
        {
            RespuestaSimple respuesta = new RespuestaSimple();
            try
            {
                PruebasCrudDAO _DAO = new PruebasCrudDAO(UserId, appSettings.Value.ConnectionStrings["CnxBDPruebas"],"");

            }
            catch (Exception)
            {

                throw;
            }
            return respuesta;
        }

    }
}
