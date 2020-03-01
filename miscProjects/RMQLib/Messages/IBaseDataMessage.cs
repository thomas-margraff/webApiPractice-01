using System;
using System.Collections.Generic;
using System.Text;

namespace RMQLib.Messages
{
    public interface IBaseDataMessage
    {
        public DateTime TimeStamp { get; set; }
        public string RoutingQueueName { get; set; }
        public string RoutingKey { get; set; }

    }
}
