using System;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Microsoft.Extensions.Configuration;

namespace telefonrehber.Module.Services
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmail(string toEmail, string subject, string message)
        {
            var smtpServer = _configuration["EmailSettings:SmtpServer"];
            var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"]);
            var username = _configuration["EmailSettings:Username"];
            var password = _configuration["EmailSettings:Password"];

            try
            {
                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress("Telefon Rehber", username));
                emailMessage.To.Add(new MailboxAddress("", toEmail));
                emailMessage.Subject = subject;
                emailMessage.Body = new TextPart("html") { Text = message };

                using (var client = new MailKit.Net.Smtp.SmtpClient()) // Doðru MailKit SmtpClient kullanýyoruz
                {
                    await client.ConnectAsync(smtpServer, smtpPort, SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(username, password);
                    await client.SendAsync(emailMessage);
                    await client.DisconnectAsync(true);
                }

                Console.WriteLine("E-posta baþarýyla gönderildi!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"E-posta gönderme hatasý: {ex.Message}");
                throw;
            }
        }
    }
}
