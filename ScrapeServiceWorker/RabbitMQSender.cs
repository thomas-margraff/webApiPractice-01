using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMQLib
{
    public class RabbitMQSender
    {
        public void Send(string queueName, string routingKeyName, string message)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "gull-01.rmq.cloudamqp.com",
                VirtualHost = "noekmbda",
                UserName = "noekmbda",
                Password = "jtKtg3LU2uEQOCJRpEgMhdSqf2N7S_Cv"
            };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "scrapeFileNotification", 
                                     durable: false, 
                                     exclusive: false, 
                                     autoDelete: false, 
                                     arguments: null);

                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "", routingKey: "scrapeFileNotification", basicProperties: null, body: body);
                // Console.WriteLine(" [x] Sent {0}", message);
            }

            //Console.WriteLine(" Press [enter] to exit.");
            //Console.ReadLine();
        }
    }
}
