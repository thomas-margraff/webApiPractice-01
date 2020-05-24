using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using RMQLib.Messages;
using RMQLib.Models;

namespace RMQLib
{
    public class RabbitContext
    {
        public RabbitConnection Connection { get; set; }
        public RabbitExchange Exchange { get; set; }
        public RabbitQueue Queue { get; set; }
        public RabbitBinder Binder { get; set; }
        public List<RmqSender> Senders { get; set; }
        public List<RMQReceiver> Receivers { get; set; }

        public RabbitContext()
        {
            this.Exchange = new RabbitExchange();
            this.Queue = new RabbitQueue();
            this.Binder = new RabbitBinder();
            this.Senders = new List<RmqSender>();
            this.Receivers = new List<RMQReceiver>();
        }

        public IConnection Connect()
        {
            RabbitConnection options = new RabbitConnection();
            ConnectionFactory factory = new ConnectionFactory();
            factory.UserName = Connection.UserName;
            factory.Password = Connection.Password;
            factory.VirtualHost = Connection.VirtualHost;
            factory.HostName = Connection.HostName;

            IConnection conn = factory.CreateConnection();

            return conn;
        }

        // not used yet...
        public void SendAll(RabbitMessage msg)
        {
            foreach (var sender in this.Senders)
            {
                sender.Send(msg);
            }
        }
    }

}

