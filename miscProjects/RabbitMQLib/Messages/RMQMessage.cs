using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQLib.Messages
{
    public class RMQMessage : BaseMessage
    {
        public RMQMessage() : base()
        {
        }

        public RMQMessage(string queueName, string routingKeyName)
        {
            this.QueueName = queueName;
            this.RoutingKeyName = routingKeyName;
        }

        public RMQMessage(string queueName, string routingKeyName, string message) 
        {
            this.QueueName = queueName;
            this.RoutingKeyName = routingKeyName;
            this.Message = message;
        }
    }
}
