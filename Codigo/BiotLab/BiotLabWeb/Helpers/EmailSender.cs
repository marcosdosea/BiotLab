using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;

namespace BiotLabWeb.Helpers
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailSender> _logger;

        public EmailSender(IConfiguration configuration, ILogger<EmailSender> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var host = GetRequiredConfig("EmailSettings:Host", "Smtp:Host");
            var port = GetIntConfig(587, "EmailSettings:Port", "Smtp:Port");
            var username = GetRequiredConfig("EmailSettings:Username", "Smtp:Username");
            var password = GetRequiredConfig("EmailSettings:Password", "Smtp:Password");
            var fromEmail = GetRequiredConfig("EmailSettings:FromEmail", "Smtp:From");
            var fromName = GetOptionalConfig("BiotLab", "EmailSettings:FromName", "Smtp:FromName");
            var enableSsl = GetBoolConfig(true, "EmailSettings:EnableSsl", "Smtp:EnableSsl");

            using var client = new SmtpClient(host, port)
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(username, password),
                EnableSsl = enableSsl,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Timeout = 30000
            };

            using var message = new MailMessage
            {
                From = new MailAddress(fromEmail, fromName),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true
            };

            message.To.Add(email);

            try
            {
                await client.SendMailAsync(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha ao enviar e-mail para {Email} usando o servidor SMTP {Host}:{Port}.", email, host, port);
                throw new InvalidOperationException("Não foi possível enviar o e-mail. Verifique as configurações SMTP do BiotLab.", ex);
            }
        }

        private string GetRequiredConfig(params string[] keys)
        {
            foreach (var key in keys)
            {
                var value = _configuration[key];
                if (!string.IsNullOrWhiteSpace(value))
                {
                    return value.Trim();
                }
            }

            throw new InvalidOperationException($"Configuração de e-mail ausente. Informe uma destas chaves: {string.Join(", ", keys)}.");
        }

        private string GetOptionalConfig(string defaultValue, params string[] keys)
        {
            foreach (var key in keys)
            {
                var value = _configuration[key];
                if (!string.IsNullOrWhiteSpace(value))
                {
                    return value.Trim();
                }
            }

            return defaultValue;
        }

        private int GetIntConfig(int defaultValue, params string[] keys)
        {
            foreach (var key in keys)
            {
                var value = _configuration[key];
                if (int.TryParse(value, out var parsed))
                {
                    return parsed;
                }
            }

            return defaultValue;
        }

        private bool GetBoolConfig(bool defaultValue, params string[] keys)
        {
            foreach (var key in keys)
            {
                var value = _configuration[key];
                if (bool.TryParse(value, out var parsed))
                {
                    return parsed;
                }
            }

            return defaultValue;
        }
    }
}
