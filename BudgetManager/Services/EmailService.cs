
using System.Net;
using System.Net.Mail;

namespace BudgetManager.Services
{
    public class EmailService: IEmailService
    {
        private readonly IConfiguration _configuration;
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailForgotPassword(string receiver, string link)
        {
            var email = _configuration.GetValue<string>("EmailConfiguration:Email");
            var password = _configuration.GetValue<string>("EmailConfiguration:Password");
            var host = _configuration.GetValue<string>("EmailConfiguration:Host");
            var port = _configuration.GetValue<int>("EmailConfiguration:Port");

            var client = new SmtpClient(host, port)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(email, password)
            };

            var transmitter = email!;
            var subject = "¿Has olvidado tu contraseña?";
            var htmlContent =
                $@"
                    Solicitud de cambio de contraseña

                    Saludos

                    Este mensaje le llega porque usted ha solicitado un cambio de contraseña. Si esta solicitud no fue
                    hecha por usted, haga caso omiso de este mensaje.

                    Para cambiar su contraseña, haga click en el siguiente enlace:

                    {link}
                        
                    Atentamente, 
                    Equipo Administración de Presupuesto.
                       
                ";
            var message = new MailMessage(transmitter, receiver, subject, htmlContent);
            await client.SendMailAsync(message);
        }
    }
}
