using System;
using System.Collections.Generic;
using System.Text;
using static System.Console;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQLib.Messages;

namespace RabbitMQLib
{
    public class RMQReceiver
    {
        public void Receive(RMQMessage rmqMsg)
        {
            WriteLine("started receiver");

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

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    // WriteLine(" [x] Received {0}", message);
                };
                channel.BasicConsume(queue: rmqMsg.QueueName, autoAck: true, consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }
    }
}
