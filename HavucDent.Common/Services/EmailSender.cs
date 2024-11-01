using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Configuration;

namespace HavucDent.Common.Services
{
    public class EmailSender : IEmailSender
    {

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        private readonly IConfiguration _configuration;

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var smtpServer = _configuration["EmailSettings:SmtpServer"];
            var port = int.Parse(_configuration["EmailSettings:Port"]);
            var senderEmail = _configuration["EmailSettings:SenderEmail"];
            var password = _configuration["EmailSettings:Password"];

            using var client = new SmtpClient(smtpServer)
            {
                Port = port,
                EnableSsl = true,
                Credentials = new NetworkCredential(senderEmail, password),
                
                
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(senderEmail),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true,
            }; 
            mailMessage.To.Add(email);

            await client.SendMailAsync(mailMessage);
        }
    }
}