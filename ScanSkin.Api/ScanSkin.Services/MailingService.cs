using Microsoft.Extensions.Options;
using MimeKit;
using ScanSkin.Api.Setting;
using ScanSkin.Core.Service.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;

namespace ScanSkin.Services
{
    
    public class MailingService : IMailingService
    {
        private readonly MailSetting _mailingSetting;   
        public MailingService(IOptions<MailSetting> mailSetting)
        {
            _mailingSetting = mailSetting.Value;
        }

        public  async Task SendEmailAsync(string Mailto , string ConfirmationCode)
        {
            var email = new MimeMessage
            {
                Sender = MailboxAddress.Parse(_mailingSetting.Email),
                Subject = "SkanSkinApp",
            };

            email.To.Add(MailboxAddress.Parse(Mailto));
            var builder = new BodyBuilder();
            builder.HtmlBody = ConfirmationCode;    
            email.Body = builder.ToMessageBody();
            email.From.Add(new MailboxAddress(_mailingSetting.DisplayName , _mailingSetting.Email));
            using var smtp = new SmtpClient();
            smtp.Connect(_mailingSetting.Host, _mailingSetting.Port);
            smtp.Authenticate(_mailingSetting.Email, _mailingSetting.password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}
