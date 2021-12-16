using System.Collections.Generic;
using MimeKit;

namespace HousingAssociation.Models
{
    public class MailMessage
    {
        public MailMessage(MailboxAddress from, IEnumerable<MailboxAddress> receivers, bool isMessageHtml = false)
        {
            From = from;
            Receivers = receivers;
            IsMessageHtml = isMessageHtml;
        }
        public MailboxAddress From { get; set; }
        public IEnumerable<MailboxAddress> Receivers { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public bool IsMessageHtml { get; set; }
    }
}