using System;
using System.Collections.Generic;
using System.Text;

namespace RMQLib.Messages
{
    // https://developpaper.com/the-correct-way-to-use-rabbitmq-in-net-core/
    public class RMQMessage : BaseMessage
    {
        public RMQMessage() : base()
        {
        }

        public RMQMessage(string exchange, string queueName, string routingKeyName)
        {
            this.ExchangeName = exchange;
            this.QueueName = queueName;
            this.RoutingKeyName = routingKeyName;
        }

        public RMQMessage(string exchange, string queueName, string routingKeyName, string message)
        {
            this.QueueName = queueName;
            this.RoutingKeyName = routingKeyName;
            this.Message = message;
        }


    }
}
