using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQLib.Messages
{
    public class BaseMessage
    {
        public string QueueName { get; set; }
        public string RoutingKeyName { get; set; }
        public string Message { get; set; }
        public string HostName { get; set; }
        public string VirtualHost { get; set; }
        public string Exchange { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public DateTime DateTimeSent { get; set; }

        public BaseMessage()
        {
            this.Exchange = "";
            this.HostName = "gull-01.rmq.cloudamqp.com";
            this.VirtualHost = "noekmbda";
            this.UserName = "noekmbda";
            this.Password = "jtKtg3LU2uEQOCJRpEgMhdSqf2N7S_Cv";
            this.DateTimeSent = DateTime.Now;
        }

        public BaseMessage(string queueName, string routingKeyName, string message) : base()
        {
            this.QueueName = queueName;
            this.RoutingKeyName = routingKeyName;
            this.Message = message;
            this.DateTimeSent = DateTime.Now;
        }
    }
}
