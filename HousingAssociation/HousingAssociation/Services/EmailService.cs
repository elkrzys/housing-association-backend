using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using HousingAssociation.Models;
using HousingAssociation.Settings;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace HousingAssociation.Services
{
    public class EmailService
    {
        private readonly MailSettings _mailSettings;
        
        public EmailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }
        public string GetAppMailAddress => _mailSettings.User;
        public void SendEmail(MailMessage message)
        {
            var email = new MimeMessage();
            email.From.Add(message.From);
            email.To.AddRange(message.Receivers);
            email.Subject = message.Subject;
            
            email.Body = new TextPart(message.IsMessageHtml ? TextFormat.Html : TextFormat.Plain)
                {Text = message.Message};
            
            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.User, _mailSettings.Password);
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}