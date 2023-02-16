using AngularStore.Core.Configuration;
using AngularStore.Core.Interfaces;
using AngularStore.Core.Models;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;

namespace AngularStore.Core.Services
{
    public class MessageService : IMessageService
    {
        private readonly MailSettings _settings;

        public MessageService(IOptions<MailSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task<bool> SendMessage(MailData mailData)
        {
            try
            {
                // Initialize a new instance of the MimeKit.MimeMessage class
                var mail = new MimeMessage();

                // Sender
                mail.From.Add(new MailboxAddress(_settings.DisplayName, mailData.From ?? _settings.From));
                mail.Sender = new MailboxAddress(mailData.DisplayName ?? _settings.DisplayName, mailData.From ?? _settings.From);

                // Receiver
                foreach (string mailAddress in mailData.To)
                    mail.To.Add(MailboxAddress.Parse(mailAddress));

                // Add Content to Mime Message
                var body = new BodyBuilder();
                mail.Subject = mailData.Subject;
                body.HtmlBody = mailData.Body;
                mail.Body = body.ToMessageBody();

                using var smtp = new SmtpClient();

                if (_settings.UseSSL)
                {
                    await smtp.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.SslOnConnect);
                }
                else if (_settings.UseStartTls)
                {
                    await smtp.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.StartTls);
                }
                await smtp.AuthenticateAsync(_settings.UserName, _settings.Password);
                await smtp.SendAsync(mail);
                await smtp.DisconnectAsync(true);

                return true;

            }
            catch (Exception exception)
            {
                return false;
            }
        }
    }
}
