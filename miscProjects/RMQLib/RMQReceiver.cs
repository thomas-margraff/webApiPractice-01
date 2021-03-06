﻿using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RMQLib.Messages;
using static System.Console;

namespace RMQLib
{
    // https://developpaper.com/the-correct-way-to-use-rabbitmq-in-net-core/

    public class RMQReceiver
    {
        private RabbitContext ctx;
        private readonly IConnection connection;
        private readonly IModel channel;

        public RMQReceiver(RabbitContext ctx)
        {
            this.ctx = ctx;

            try
            {
                var factory = new ConnectionFactory()
                {
                    VirtualHost = ctx.Connection.VirtualHost,
                    HostName = ctx.Connection.HostName,
                    UserName = ctx.Connection.UserName,
                    Password = ctx.Connection.Password
                };

                this.connection = Connection.Connect(this.ctx);
                this.channel = connection.CreateModel();
            }
            catch (Exception ex)
            {
                Console.WriteLine("RabbitListener init error ex:{0}", ex.Message);
                throw ex;
            }
        }

        public void SetRoutingKey(string queueName, string routingKey)
        {
            ctx.Binder.RoutingKey = routingKey;
            ctx.Binder.QueueName = queueName;
            ctx.Queue.Name = queueName;
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

        public void Listen()
        {
            this.Register();
        }

        public void Register<T>(T msg) where T : IBaseDataMessage
        {
            this.Register(msg.RoutingQueueName, msg.RoutingKey);
        }

        public void Register(string routingQueueName, string routingKey)
        {
            this.SetRoutingKey(routingQueueName, routingKey);
            Register();
        }

        // Registered Consumer Monitor Here
        public void Register()
        {
            if (!string.IsNullOrWhiteSpace(ctx.Queue.Name))
            {
                if (!string.IsNullOrWhiteSpace(ctx.Exchange.Name))
                {
                    channel.ExchangeDeclare(exchange: ctx.Exchange.Name,
                                        durable: ctx.Exchange.Durable,
                                        type: ctx.Exchange.Type);
                }
            }
            else
            {
                channel.ExchangeDeclare(exchange: ctx.Exchange.Name, durable: ctx.Exchange.Durable, type: ctx.Exchange.Type);
            }

            if (!string.IsNullOrWhiteSpace(ctx.Queue.Name))
            {
                channel.QueueDeclare(queue: ctx.Queue.Name,
                                     durable: ctx.Queue.Durable,
                                     exclusive: ctx.Queue.Exclusive,
                                     autoDelete: ctx.Queue.AutoDelete,
                                     arguments: ctx.Queue.Arguments);
            }
            else
            {
                ctx.Queue.Name = channel.QueueDeclare().QueueName;
                ctx.Binder.QueueName = ctx.Queue.Name;
            }

            if (!string.IsNullOrWhiteSpace(ctx.Exchange.Name))
            {
                channel.QueueBind(queue: ctx.Binder.QueueName,
                              exchange: ctx.Binder.ExchangeName,
                              routingKey: ctx.Binder.RoutingKey);
            }

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
            {
                //var body = ea.Body.ToString();
                //var message = Encoding.UTF8.GetString(body);
                var message = ea.Body.ToString();
                var result = Process(message);
                if (result)
                {
                    channel.BasicAck(ea.DeliveryTag, false);
                }
            };

            consumer.Registered += (model, ea) =>
            {
                var tag = ea.ConsumerTags;
                WriteLine("registered tag: {0}", tag);
            };
            consumer.Shutdown += (model, ea) =>
            {
                var cause = ea.Cause;
                WriteLine("shutdown cause {0}", cause);
            };
            consumer.Unregistered += (model, ea) =>
            {
                var tag = ea.ConsumerTags;
                WriteLine("Unregistered tag: {0}", tag);
            };

            channel.BasicConsume(queue: ctx.Queue.Name, consumer: consumer);
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
