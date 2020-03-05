using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace RMQLib.Models
{
    /* Exchange Types
    Direct = "direct";
    Fanout = "fanout";
    Headers = "headers";
    Topic = "topic";          
    */

    public class RabbitExchange
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public bool Durable { get; set; }
        public bool AutoDelete { get; set; }
        public IDictionary<string, object> Arguments { get; set; } = null;
        public List<RabbitQueue> Queues { get; set; }

        public RabbitExchange()
        {
            Queues = new List<RabbitQueue>();
        }
    }
}
