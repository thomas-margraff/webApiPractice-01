using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;
using System.Text.Json;

namespace RMQLib
{
    // https://developpaper.com/the-correct-way-to-use-rabbitmq-in-net-core/

    public class SenderNew
    {
        private RabbitContext ctx;
        private IConnection connection;

        public SenderNew(RabbitContext ctx) { this.ctx = ctx; }

        private IConnection connect()
        {
            this.connection = Connection.Connect();
            return this.connection;
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
                channel.ExchangeDeclare("cv.scraper.exchange", ExchangeType.Direct, true, false, null);
                channel.QueueDeclare("cv.scraper.queue", true, false, false, null);
                channel.QueueDeclare("", true, false, false, null);

                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: ctx.Binder.ExchangeName,
                                     routingKey: ctx.Binder.RoutingKey,
                                     basicProperties: null,
                                     body: body);
            }
        }
    }
}
