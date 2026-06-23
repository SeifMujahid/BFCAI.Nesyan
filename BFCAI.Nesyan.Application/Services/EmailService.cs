using BFCAI.Nesyan.Application.Abstraction.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            // For now, if no SMTP settings are configured, we just print to console to simulate sending
            var host = _configuration["EmailSettings:Host"];
            if (string.IsNullOrEmpty(host))
            {
                Console.WriteLine("================ EMAIL SIMULATION ================");
                Console.WriteLine($"To: {toEmail}");
                Console.WriteLine($"Subject: {subject}");
                Console.WriteLine($"Body: {body}");
                Console.WriteLine("==================================================");
                return;
            }

            try
            {
                var port = int.Parse(_configuration["EmailSettings:Port"] ?? "587");
                var enableSsl = bool.Parse(_configuration["EmailSettings:EnableSsl"] ?? "true");
                var username = _configuration["EmailSettings:Username"];
                var password = _configuration["EmailSettings:Password"];
                var fromEmail = _configuration["EmailSettings:FromEmail"];

                using var client = new SmtpClient(host, port)
                {
                    Credentials = new NetworkCredential(username, password),
                    EnableSsl = enableSsl
                };

                var mailMessage = new MailMessage(fromEmail, toEmail, subject, body)
                {
                    IsBodyHtml = true
                };

                await client.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email: {ex.Message}");
                // In a real app we might log this properly or throw, but here we just suppress to prevent blocking
            }
        }
    }
}
