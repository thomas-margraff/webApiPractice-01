using RMQLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RMQLib
{
    public class RMQSession
    {
        public RabbitContext RabbitContext { get; set; }
        public string ConfigFileName { get; set; }
        public RMQContext Context { get; set; }
        public RmqSender Sender { get; set; }
        public RMQReceiver Receiver { get; set; }

        public RMQSession(string configFileName)
        {
            ConfigFileName = configFileName;
            Context = new RMQContext();
        }

        public void Create(string connectionName, string exchangeName, string queueName, string routingKey)
        {
            Context = new RMQContext().Create(ConfigFileName);

            RabbitContext = new RabbitContext();

            RabbitContext.Connection = Context.Connections.Where(r => r.Name == connectionName).FirstOrDefault();
            RabbitContext.Exchange = Context.Exchanges.Where(r => r.Name == exchangeName).FirstOrDefault();
            RabbitContext.Queue = Context.Queues.Where(r => r.Name == queueName).FirstOrDefault();
            RabbitContext.Binder = new RabbitBinder()
            {
                ExchangeName = RabbitContext.Exchange.Name,
                QueueName = RabbitContext.Queue.Name,
                RoutingKey = routingKey
            };

            Sender = new RmqSender(RabbitContext);
            Sender.SetRoutingKey(RabbitContext.Queue.Name, RabbitContext.Binder.RoutingKey);

            // Receiver = new RMQReceiver(_ctx);

        }
    }
}
