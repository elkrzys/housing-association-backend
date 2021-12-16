using System.Collections.Generic;
using MimeKit;

namespace HousingAssociation.Models
{
    public class MailAddress
    {
        public MailAddress(string name, string address)
        {
            Name = name;
            Address = address;
        }
        public string Name { get; set; }
        public string Address { get; set; }
    };
    
    public class MailContent
    {
        public MailContent(string subject, string message)
        {
            Subject = subject;
            Message = message;
        }
        public string Subject { get; set; }
        public string Message { get; set; }
    };
    
    public class MailMessage
    {
        public MailMessage(MailAddress from, IEnumerable<string> receivers, MailContent content, bool isMessageHtml)
        {
            From = new MailboxAddress(from.Name, from.Address);
            Receivers = receivers;
            IsMessageHtml = isMessageHtml;
            Subject = content.Subject;
            Message = content.Message;
        }
        public MailboxAddress From { get; set; }
        public IEnumerable<string> Receivers { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public bool IsMessageHtml { get; set; }
    }
}