﻿using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RMQLib.Messages;
using System.Text.Json;
using System.Text.Json.Serialization;
using RMQLib.Models;

namespace RMQLib
{
    public class Connection
    {
        public static IConnection Connect()
        {
            RabbitConnection options = new RabbitConnection();
            ConnectionFactory factory = new ConnectionFactory();
            factory.UserName = options.UserName;
            factory.Password = options.Password;
            factory.VirtualHost = options.VirtualHost;
            factory.HostName = options.HostName;

            IConnection conn = factory.CreateConnection();

            return conn;
        }
    }
}
