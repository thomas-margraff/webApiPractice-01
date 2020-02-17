using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Console;

namespace ConsoleLogger
{
    public class RabbitMQReceiver
    {
        public void Receive()
        {
            WriteLine("started receiver");
            string listenOnQueue = "cvConsoleLogger";
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
                channel.QueueDeclare(queue: listenOnQueue, durable: false, exclusive: false, autoDelete: false, arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    WriteLine(message);
                    
                };
                channel.BasicConsume(queue: listenOnQueue, autoAck: true, consumer: consumer);

                Console.WriteLine("Log receiver started. Press [enter] to exit.");
                Console.ReadLine();
            }
        }

    }
}
