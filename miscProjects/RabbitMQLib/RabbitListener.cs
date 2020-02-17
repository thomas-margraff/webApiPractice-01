using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQLib.Messages;

namespace RabbitMQLib
{
    // https://developpaper.com/the-correct-way-to-use-rabbitmq-in-net-core/

    public class RabbitListener
    {
        private readonly IConnection connection;
        private readonly IModel channel;
        private RMQMessage _options;

        public RabbitListener(RMQMessage options)
        {
            try
            {
                var factory = new ConnectionFactory()
                {
                    VirtualHost = options.VirtualHost,
                    HostName = options.HostName,
                    UserName = options.UserName,
                    Password = options.Password
                };

                this.connection = factory.CreateConnection();
                this.channel = connection.CreateModel();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"RabbitListener init error,ex:{ex.Message}");
            }
            this._options = options;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Register();
            return Task.CompletedTask;
        }

        // Method of handling messages
        public virtual bool Process(string message)
        {
            throw new NotImplementedException();
        }

        // Registered Consumer Monitor Here
        public void Register()
        {
            Console.WriteLine($"RabbitListener register,routeKey:{_options.RoutingKeyName}");
            channel.ExchangeDeclare(exchange: "message", type: "topic");
            channel.QueueDeclare(queue: _options.QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
            channel.QueueBind(queue: _options.QueueName,
                              exchange: "message",
                              routingKey: _options.RoutingKeyName);
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
                var result = Process(message);
                if (result)
                {
                    channel.BasicAck(ea.DeliveryTag, false);
                }
            };
            channel.BasicConsume(queue: _options.QueueName, consumer: consumer);
        }

        public void DeRegister()
        {
            this.connection.Close();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            this.connection.Close();
            return Task.CompletedTask;
        }
    }
}
