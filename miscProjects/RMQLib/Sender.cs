using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;
using System.Text.Json;
using RMQLib.Messages;

namespace RMQLib
{
    // https://developpaper.com/the-correct-way-to-use-rabbitmq-in-net-core/

    public class Sender
    {
        private RabbitContext ctx;
        private IConnection connection;

        public Sender(RabbitContext ctx) { this.ctx = ctx; }

        private IConnection connect()
        {
            this.connection = Connection.Connect(this.ctx);
            return this.connection;
        }

        public void SetRoutingKey(string queueName, string routingKey)
        {
            ctx.Binder.RoutingKey = routingKey;
            ctx.Binder.QueueName = queueName;
            ctx.Queue.Name = queueName;
        }

        public void SendMessage<T>(T obj) where T : IBaseDataMessage
        {
            ctx.Binder.RoutingKey = obj.RoutingKey;
            ctx.Binder.QueueName = obj.RoutingQueueName;
            ctx.Queue.Name = obj.RoutingQueueName;

            string json = JsonSerializer.Serialize(obj);
            Send(json);
        }

        public void Send<T>(T obj)
        {
            string json = JsonSerializer.Serialize(obj);
            Send(json);
        }

        public void Send(string message)
        {
            // if (this.connection == null)
            this.connect();

            using (var connection = this.connection)
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: ctx.Exchange.Name,
                                        durable: ctx.Exchange.Durable,
                                        type: ctx.Exchange.Type);  // ExchangeType.Fanout);

                if (!string.IsNullOrWhiteSpace(ctx.Queue.Name))
                {
                    channel.QueueDeclare(queue: ctx.Queue.Name,
                                         durable: ctx.Queue.Durable,
                                         exclusive: ctx.Queue.Exclusive,
                                         autoDelete: ctx.Queue.AutoDelete,
                                         arguments: ctx.Queue.Arguments);
                }

                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: ctx.Binder.ExchangeName,
                                     routingKey: ctx.Binder.RoutingKey,
                                     basicProperties: null,
                                     body: body);
            }
        }
    }
}
