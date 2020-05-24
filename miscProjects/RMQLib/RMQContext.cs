using RMQLib.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RMQLib
{
    public class RMQContext
    {
        public List<RabbitConnection> Connections { get; set; }
        public List<RabbitExchange> Exchanges { get; set; }
        public List<RabbitQueue> Queues { get; set; }

        public RMQContext()
        {
            Connections = new List<RabbitConnection>();
            Exchanges = new List<RabbitExchange>();
            Queues = new List<RabbitQueue>();
        }
    }

    public class RMQConnection
    {
        public string Name { get; set; }
        public string HostName { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string VirtualHost { get; set; }
        public bool AutomaticRecoveryEnabled { get; set; }
        public bool TopologyRecoveryEnabled { get; set; }
        public int RequestedConnectionTimeout { get; set; }
        public ushort RequestedHeartbeat { get; set; }

        public RMQConnection()
        {
        }
    }

    public class RMQExchange
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public bool Durable { get; set; }
        public bool AutoDelete { get; set; }
        public IDictionary<string, object> Arguments { get; set; } = null;

        public RMQExchange()
        {
            
        }
    }

    public class RMQQueue
    {
        public string Name { get; set; }
        public bool Durable { get; set; }
        public bool Exclusive { get; set; }
        public bool AutoDelete { get; set; }
        public IDictionary<string, object> Arguments { get; set; } = null;

        public RMQQueue()
        {
        }
    }

}
