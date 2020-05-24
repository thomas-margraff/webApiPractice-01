using System;
using System.Collections.Generic;
using System.Text;

namespace RMQLib.Messages
{
    public class BaseDataMessage : IBaseDataMessage
    {
        public DateTime TimeStamp { get; set; } = DateTime.Now;
        
        public string ExchangeName { get; set; }
        public string RoutingQueueName { get; set; }
        public string RoutingKey { get; set; }
        public string ApplicationName { get; set; } = "";
    }
}
