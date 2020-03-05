using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using RMQLib.Models;

namespace RMQLib
{
    public static class RabbitContextExtensions
    {
        public static RabbitContext Create(this RabbitContext ctx, string exchange, string exchangeType, string routingKey = "")
        {
            ctx = new RabbitContext();
            
            ctx.Connection = new RabbitConnection();
            ctx.Connection.ClearDefaults();
            ctx.Connection.HostName = "localhost";
            ctx.Exchange.Name = exchange;
            ctx.Exchange.Type= exchangeType;
            ctx.Binder.ExchangeName = exchange;
            ctx.Binder.RoutingKey = routingKey;
            return ctx;
        }
        public static RabbitContext Create(this RabbitContext ctx, string fileName)
        {
            ctx = new RabbitContext();

            var filePath = Path.Combine(@"Configuration", fileName);
            string fullFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filePath);
            string json = File.ReadAllText(fullFilePath);

            ctx = JsonConvert.DeserializeObject<RabbitContext>(json);

            return ctx;
        }
    }
}
