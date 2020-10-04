using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using static System.Console;

namespace Consumer
{
    public class Receiver
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare("BasicTest", false, false, false, null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    int i = 0;
                    //var body = ea.Body;
                    //var message = Encoding.UTF8.GetString(body);
                    //WriteLine("received message {0}...", message);
                };

                channel.BasicConsume("BasicTest", true, consumer);

                WriteLine("Press [enter] to exit the Consumer");
                ReadLine();
            }
        }
    }
}
