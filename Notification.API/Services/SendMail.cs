using MailKit.Net.Imap;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MimeKit;
using Notification.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace Notification.API.Services
{
    public interface ISendMail
    {
        Task<bool> SendEmail(SendMailRequest request);
    }
    public class SendMail : ISendMail
    {
        private readonly SmtpSettings _smtpSettings;
        private readonly IWebHostEnvironment _env;
        public SendMail(IOptions<SmtpSettings> smtpSettings, IWebHostEnvironment env)
        {
            _smtpSettings = smtpSettings.Value;
            _env = env;
        }

        public async Task<bool> SendEmail(SendMailRequest request)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_smtpSettings.SenderName, _smtpSettings.SenderEmail));
                message.To.Add(MailboxAddress.Parse(request.Email));
                message.Subject = request.Subject;
                message.Body = new TextPart("html")
                {
                    Text = request.Body
                };

                using (var client = new SmtpClient())
                {
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    //client.SslProtocols = SslProtocols.Ssl3 | SslProtocols.Tls | SslProtocols.Tls11 | SslProtocols.Tls12 | SslProtocols.Tls13;

                    if (_env.IsDevelopment())
                    {
                        await client.ConnectAsync(_smtpSettings.Server, _smtpSettings.Port, false);
                    }
                    else
                    {
                        await client.ConnectAsync(_smtpSettings.Server);
                    }

                    await client.AuthenticateAsync(_smtpSettings.UserName, _smtpSettings.Password);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("===" + e.ToString());
            }
            return false;
        }
    }
}
