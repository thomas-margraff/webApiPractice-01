using System;
using System.Collections.Generic;
using System.Text;

namespace EasyNetQ.Common.Messages
{
    public class EmailMessage : BaseEmailMessage
    {
        public string FromEmail { get; set; } = "tmargraff@gmail.com";
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<string> Recipients { get; set; }
        public bool IsBodyHtml { get; set; } = false;

        public EmailMessage()
        {
            Recipients = new List<string>();
        }
    }
}
