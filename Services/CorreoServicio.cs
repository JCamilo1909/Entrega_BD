using System.Net;
using System.Net.Mail;

namespace SistemaRegistros.Services
{
    public class CorreoServicio
    {
        private readonly IConfiguration _config;

        public CorreoServicio(IConfiguration config)
        {
            _config = config;
        }

        public void Enviar(string destinatario, string asunto, string cuerpoHtml)
        {
            try
            {
                var correoRemitente = _config["ConfiguracionCorreo:CorreoRemitente"];
                var contrasena = _config["ConfiguracionCorreo:ContrasenaApp"];
                var host = _config["ConfiguracionCorreo:Host"];
                var puerto = int.Parse(_config["ConfiguracionCorreo:Puerto"] ?? "587");

                var mensaje = new MailMessage();
                mensaje.From = new MailAddress(correoRemitente!, "Sistema PRUEBA");
                mensaje.To.Add(destinatario);
                mensaje.Subject = asunto;
                mensaje.Body = cuerpoHtml;
                mensaje.IsBodyHtml = true;

                using var clienteSmtp = new SmtpClient(host, puerto);
                clienteSmtp.Credentials = new NetworkCredential(correoRemitente, contrasena);
                clienteSmtp.EnableSsl = true;
                clienteSmtp.Send(mensaje);
            }
            catch (Exception ex)
            {
                // Si falla el envio, lo registramos pero no rompemos la aplicacion
                Console.WriteLine($"Error al enviar correo: {ex.Message}");
            }
        }
    }
}