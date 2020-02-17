using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQLib.Messages;

namespace RabbitMQLib
{
    public class RMQSender
    {
        public static void Send(RMQMessage rmqMsg, string message)
        {
            rmqMsg.Message = message;
            Send(rmqMsg);
        }

        public static void Send(RMQMessage rmqMsg)
        {
            var factory = new ConnectionFactory()
            {
                HostName = rmqMsg.HostName,
                VirtualHost = rmqMsg.VirtualHost,
                UserName = rmqMsg.UserName,
                Password = rmqMsg.Password
            };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: rmqMsg.QueueName, 
                                     durable: false, 
                                     exclusive: false, 
                                     autoDelete: false, 
                                     arguments: null);

                var body = Encoding.UTF8.GetBytes(rmqMsg.Message);

                channel.BasicPublish(exchange: rmqMsg.Exchange, 
                                     routingKey: rmqMsg.RoutingKeyName, 
                                     basicProperties: null, 
                                     body: body);

                // Console.WriteLine(" [x] Sent {0}", rmqMsg.Message);
            }
        }
    }
}
