using Microsoft.Extensions.Options;
using Models;
using Models.Models.MensajeriVM;
using Models.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Seguridad = Utilidades;
namespace Negocio.Trax
{
    public class NegocioMensajeria
    {
        private String _rutaArchivos = String.Empty;
        private String usuariosec = String.Empty;
        private String _correoSalida = String.Empty;
        private String _alias = String.Empty;
        private Boolean _SecureSSL = false;
        private String _servicioSMTP = String.Empty;
        private int _puertoSMTP = 0;
        private String _usuario = String.Empty;
        private String _password = String.Empty;
        private string from = String.Empty;
        private String Token = String.Empty;
        private String _archivoPlantilla = String.Empty;
        public String archivoPlantilla { get => _archivoPlantilla; set => _archivoPlantilla = value; }
        public String plantilla { get; set; }
        private String _rutaPlantillas = String.Empty;
        public String rutaPlantillas { get => _rutaPlantillas; set => _rutaPlantillas = value; }
        public void setMailConfig(String pUsuarioSec, String pcorreoSalida, String palias, Boolean pSecureSSL, String pservicioSMTP, int ppuertoSMTP, String pusuario, String ppassword, String pRutaPlantillas = "", String pPlantilla = "")
        {
            usuariosec = pUsuarioSec;
            _rutaArchivos = pRutaPlantillas;
            _correoSalida = Seguridad.Encriptacion.Desencriptar(Convert.FromBase64String(pcorreoSalida), usuariosec);
            _alias = Seguridad.Encriptacion.Desencriptar(Convert.FromBase64String(palias), usuariosec);
            _SecureSSL = pSecureSSL;
            _servicioSMTP = Seguridad.Encriptacion.Desencriptar(Convert.FromBase64String(pservicioSMTP), usuariosec);
            _puertoSMTP = ppuertoSMTP;
            _usuario = Seguridad.Encriptacion.Desencriptar(Convert.FromBase64String(pusuario), usuariosec);
            _password = Seguridad.Encriptacion.Desencriptar(Convert.FromBase64String(ppassword), usuariosec);
            if (!String.IsNullOrEmpty(pRutaPlantillas))
            {
                _rutaPlantillas = pRutaPlantillas;
                pPlantilla = String.IsNullOrEmpty(pPlantilla) ? "MACPlatillaGeneral.html" : pPlantilla;
                _archivoPlantilla = String.Concat(_rutaPlantillas, pPlantilla);
            }
        }

        /// <summary>
        /// Envía correos con o sin archivos añadidos que esten previamente en el servidor.
        /// usa una plantilla de base.
        /// </summary>
        /// <param name="to"></param>
        /// <param name="asunto"></param>
        /// <param name="mensaje"></param>
        /// <param name="Files"></param>
        /// <param name="ruta"></param>
        /// <param name="asuntoDetalle"></param>
        /// <param name="accion"></param>
        /// <returns>En caso de error regresa el texto de la excepción.</returns>
        public Respuesta SendMail(String to, String asunto, String mensaje, List<String> Files, String ruta, String asuntoDetalle, String accion)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                //Se inicializa con error
                respuesta.Status = 2;
                respuesta.Mensaje = String.Empty;
                EMailMessage eMailMessage = new EMailMessage(_correoSalida, _alias, _SecureSSL, _servicioSMTP, _puertoSMTP, _usuario, _password, ruta);
                mensaje = mensajePlantilla(asuntoDetalle, mensaje, accion);
                //respuesta.Mensaje = eMailMessage.SendMail(to, asunto, mensaje, Files);
                respuesta.Mensaje = SendMail(to, asunto, mensaje, Files);

                if (String.IsNullOrEmpty(respuesta.Mensaje))
                {
                    respuesta.Status = 1;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
                //respuesta.Mensaje = ex.Message;
            }
            return respuesta;
        }

        /// <summary>
        /// Envía correos con o sin archivos añadidos en base 64.
        /// usa una plantilla de base.
        /// </summary>
        /// <param name="to"></param>
        /// <param name="asunto"></param>
        /// <param name="mensaje"></param>
        /// <param name="Files"></param>
        /// <param name="ruta"></param>
        /// <param name="asuntoDetalle"></param>
        /// <param name="accion"></param>
        /// <returns>En caso de error regresa el texto de la excepción.</returns>
        public Respuesta SendMailBase64(String to, String asunto, String mensaje, List<ArchivoBase64> Files, String ruta, String asuntoDetalle, String accion)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                //Se inicializa con error
                respuesta.Status = 2;
                respuesta.Mensaje = String.Empty;
                EMailMessage eMailMessage = new EMailMessage(_correoSalida, _alias, _SecureSSL, _servicioSMTP, _puertoSMTP, _usuario, _password, ruta);
                mensaje = mensajePlantilla(asuntoDetalle, mensaje, accion);
                respuesta.Mensaje = eMailMessage.SendMail(to, asunto, mensaje, Files);
                if (String.IsNullOrEmpty(respuesta.Mensaje))
                {
                    respuesta.Status = 1;
                }
            }
            catch (Exception ex)
            {
                respuesta.Mensaje = ex.Message;
            }
            return respuesta;
        }
        private String mensajePlantilla(String asuntoDetalle, String detalle, String accion)
        {
            this.plantilla = Getplantilla();

            String textoPlantilla = this.plantilla;
            textoPlantilla = textoPlantilla.Replace("@asuntodetalle", asuntoDetalle);
            textoPlantilla = textoPlantilla.Replace("@detalle", detalle);
            if (!String.IsNullOrEmpty(accion))
            {
                string accionBtn = "<td align='center' width='250' style='width: 230px; padding: 5px; -webkit-border-radius: 5px;" +
                         " -moz-border-radius: 5px; border-radius: 5px;'' bgcolor='#96c057'> " +
                         accion + "</td>";
                accion = accionBtn;
            }
            textoPlantilla = textoPlantilla.Replace("@accion", accion);
            return textoPlantilla;
        }
        public void GetSettings(IOptions<AppSettings> settings, string plantilla = "")
        {
            Dictionary<string, string> mailsetting = settings.Value.MailSettings;
            String correoNotificacion = string.Empty;
            Boolean isSSL = mailsetting["SecureSSL"].Equals("true") ? true : false;
            setMailConfig(mailsetting["usuariosec"],
                mailsetting["correoSalida"],
                mailsetting["alias"], isSSL,
                mailsetting["servicioSMTP"],
                int.Parse(mailsetting["puertoSMTP"]),
                mailsetting["usuarioMail"],
                mailsetting["passMail"],
                mailsetting["rutaPlantillas"], plantilla);

        }
        private String Getplantilla()
        {
            String textoPlantilla = String.Empty;
            if (!String.IsNullOrEmpty(_archivoPlantilla))
            {
                if (System.IO.File.Exists(_archivoPlantilla))
                {
                    textoPlantilla = System.IO.File.ReadAllText(_archivoPlantilla);
                }
            }
            return textoPlantilla;
        }
        public String SendMail(String destinatario, String asunto, String mensaje, List<String> Files)
        {
            String result = String.Empty;
            try
            {
                MailMessage mail = new System.Net.Mail.MailMessage();
                mail.From = new MailAddress(_correoSalida, _alias);
                mail.To.Add(destinatario);
                mail.Subject = asunto;
                // Creamos la vista para clientes que sólo pueden acceder a texto plano
                AlternateView PlanView = AlternateView.CreateAlternateViewFromString(System.Text.RegularExpressions.Regex.Replace(mensaje, "<(.|\n)*?>", String.Empty), Encoding.UTF8, System.Net.Mime.MediaTypeNames.Text.Plain);
                // Ahora creamos la vista para clientes que pueden mostrar contenido HTML
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(mensaje, Encoding.UTF8, System.Net.Mime.MediaTypeNames.Text.Html);
                // Creamos el recurso a incrustar. referenciado desde el código HTML como origen de la imagen.

                // Vinculamos ambas vistas al mensaje
                mail.AlternateViews.Add(PlanView);
                mail.AlternateViews.Add(htmlView);
                String file = String.Empty;
                if (Files != null && Files.Count > 0)
                {
                    foreach (String f in Files)
                    {
                        file = _rutaArchivos + f;
                        Attachment attachFile = new Attachment(file);
                        mail.Attachments.Add(attachFile);
                    }
                }

                mail.Priority = MailPriority.High;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = _servicioSMTP;
                smtp.Port = _puertoSMTP;
                smtp.EnableSsl = _SecureSSL;
                smtp.Credentials = new System.Net.NetworkCredential(_usuario, _password);
                System.Net.ServicePointManager.ServerCertificateValidationCallback += delegate (Object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                smtp.Send(mail);
                mail.Dispose();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }
        public String SendMail(String destinatario, String asunto, String mensaje, List<ArchivoBase64> base64Files)
        {
            String result = String.Empty;
            try
            {
                String file = String.Empty;
                String fileName = String.Empty;
                List<String> Files = new List<string>();
                if (base64Files != null && base64Files.Count > 0)
                {
                    foreach (ArchivoBase64 f in base64Files)
                    {
                        fileName = f.nombre;
                        file = _rutaArchivos + fileName;
                        File.WriteAllBytes(file, Convert.FromBase64String(f.contenido));
                        Files.Add(fileName);
                    }
                }
                result = SendMail(destinatario, asunto, mensaje, Files);
            }
            catch (Exception ex)
            {
                result = ex.Message.ToString();
            }
            return result;
        }


    }
}

