using System;
using System.Collections.Generic;
using System.Text;

namespace RMQLib.Messages.Email
{
    public class EmailMessage : BaseDataMessage
    {
        public string FromEmail { get; set; } = "tmargraff@gmail.com";
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<string> Recipients { get; set; }
        public bool IsBodyHtml { get; set; } = false;
        public string SmtpHost { get; set; } = "smtp.gmail.com";
        public int Port { get; set; } = 587;
        public string EmailUserName { get; set; } = "tmargraff@gmail.com";
        public string EmailPassword { get; set; } = "Sapphire5211";

        public EmailMessage():
            base()
        {
            base.RoutingQueueName = "cvEmail";
            base.RoutingKey = "cvEmail";

            Recipients = new List<string>();
        }
    }
}
