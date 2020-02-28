using System;
using System.Collections.Generic;
using System.Text;

namespace EasyNetQ.Common.Messages
{
    public interface IBaseMessage
    {
        public DateTime TimeStamp { get; set; }
    }

    public class BaseMessage
    {
        public DateTime TimeStamp { get; set; } = DateTime.Now;
    }

    public class BaseEmailMessage: BaseMessage
    {
        public string SmtpHost { get; set; } = "smtp.gmail.com";
        public int Port { get; set; } = 587;
        public string UserName { get; set; } = "tmargraff@gmail.com";
        public string Password { get; set; } = "Sapphire5211";
    }

}
