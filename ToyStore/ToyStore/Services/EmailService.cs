using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration; 
using System.Threading.Tasks;

namespace ToyStore.Web.Services 
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string messageBody)
        {
            var settings = _configuration.GetSection("EmailSettings");

            var host = settings["MailServer"];
            var port = int.Parse(settings["MailPort"]);
            var email = settings["SenderEmail"];
            var password = settings["SenderPassword"];
            var name = settings["SenderName"];

            using (var client = new SmtpClient(host, port))
            {
                client.Credentials = new NetworkCredential(email, password);
                client.EnableSsl = true; 

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(email, name),
                    Subject = subject,
                    Body = messageBody,
                    IsBodyHtml = true 
                };

                mailMessage.To.Add(toEmail);

                await client.SendMailAsync(mailMessage);
            }
        }
    }
}