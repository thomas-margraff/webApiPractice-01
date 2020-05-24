using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using static System.Console;

namespace RMQLib.Messages
{
    public class ScrapeServiceWorkerSend
    {
        RabbitContext _ctx;
        RmqSender _sender;

        public ScrapeServiceWorkerSend(RabbitContext ctx)
        {
            _ctx = ctx;
            _sender = new RmqSender(_ctx);
        }

        public void Send<T>(T obj)
        {
            _ctx.Binder.RoutingKey = "doscrape";
            _ctx.Binder.QueueName = "";
            _ctx.Queue.Name = "";

            string json = JsonSerializer.Serialize(obj);
            Send(json);
        }

        public void Send(string msg)
        {
            _ctx.Binder.RoutingKey = "doscrape";
            _ctx.Binder.QueueName = "";
            _ctx.Queue.Name = "";

            _sender.Send(msg);
            
        }
    }

    public class ScrapeServiceWorkerReceive : RMQReceiver
    {
        public ScrapeServiceWorkerReceive(RabbitContext ctx) : base(ctx)
        {
            ctx.Binder.RoutingKey = "doscrape";
            ctx.Binder.QueueName = "";
            ctx.Queue.Name = "";

            this.Register();
        }

        public override bool Process(string message)
        {
            WriteLine("received " + message);
            return true;
        }

    }

}
