using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Text;
using System.Text.RegularExpressions;

namespace Models.Models.MensajeriVM
{
    public class EMailMessage
    {
        private string _correoSalida = string.Empty;
        private string _alias = string.Empty;
        private bool _SecureSSL = false;
        private string _servicioSMTP = string.Empty;
        private int _puertoSMTP = 0;
        private string _usuario = string.Empty;
        private string _password = string.Empty;
        private string _rutaArchivos = string.Empty;

        public EMailMessage()
        {
        }

        public EMailMessage(
          string pcorreoSalida,
          string palias,
          bool pSecureSSL,
          string pservicioSMTP,
          int ppuertoSMTP,
          string pusuario,
          string ppassword,
          string prutaArchivos)
        {
            this._correoSalida = pcorreoSalida;
            this._alias = palias;
            this._SecureSSL = pSecureSSL;
            this._servicioSMTP = pservicioSMTP;
            this._puertoSMTP = ppuertoSMTP;
            this._usuario = pusuario;
            this._password = ppassword;
            this._rutaArchivos = prutaArchivos;
        }

        public string SendMail(string destinatario, string asunto, string mensaje, List<string> Files)
        {
            string empty1 = string.Empty;
            try
            {
                MailMessage message = new MailMessage();
                message.From = new MailAddress(this._correoSalida, this._alias);
                message.To.Add(destinatario);
                message.Subject = asunto;
                AlternateView alternateViewFromString1 = AlternateView.CreateAlternateViewFromString(Regex.Replace(mensaje, "<(.|\n)*?>", string.Empty), Encoding.UTF8, "text/plain");
                AlternateView alternateViewFromString2 = AlternateView.CreateAlternateViewFromString(mensaje, Encoding.UTF8, "text/html");
                message.AlternateViews.Add(alternateViewFromString1);
                message.AlternateViews.Add(alternateViewFromString2);
                string empty2 = string.Empty;
                if (Files != null && Files.Count > 0)
                {
                    foreach (string file in Files)
                    {
                        Attachment attachment = new Attachment(this._rutaArchivos + file);
                        message.Attachments.Add(attachment);
                    }
                }
                message.Priority = MailPriority.High;
                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Host = this._servicioSMTP;
                smtpClient.Port = this._puertoSMTP;
                smtpClient.EnableSsl = this._SecureSSL;
                smtpClient.Credentials = (ICredentialsByHost)new NetworkCredential(this._usuario, this._password);
                ServicePointManager.ServerCertificateValidationCallback += (RemoteCertificateValidationCallback)((s, certificate, chain, sslPolicyErrors) => true);
                smtpClient.Send(message);
                message.Dispose();
            }
            catch (Exception ex)
            {
                empty1 = ex.Message.ToString();
            }
            return empty1;
        }

        public string SendMail(
          string destinatario,
          string asunto,
          string mensaje,
          List<ArchivoBase64> base64Files)
        {
            string empty1 = string.Empty;
            string str;
            try
            {
                string empty2 = string.Empty;
                string empty3 = string.Empty;
                List<string> Files = new List<string>();
                if (base64Files != null && base64Files.Count > 0)
                {
                    foreach (ArchivoBase64 base64File in base64Files)
                    {
                        string nombre = base64File.nombre;
                        System.IO.File.WriteAllBytes(this._rutaArchivos + nombre, Convert.FromBase64String(base64File.contenido));
                        Files.Add(nombre);
                    }
                }
                str = this.SendMail(destinatario, asunto, mensaje, Files);
            }
            catch (Exception ex)
            {
                str = ex.Message.ToString();
            }
            return str;
        }
    }
}
