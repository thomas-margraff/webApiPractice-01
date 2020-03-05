using System;
using RMQLib;

namespace RMQ.Test.Receiver
{
    class Program
    {
        static void Main(string[] args)
        {
            string exchange = "direct_logs";
            string exchangeType = "direct";
            string routingKey = "info";

            // direct
            // RabbitContext ctx = new RabbitContext().Create("logs", "fanout");
            // var receiver = new LogFanoutReceiver(ctx);

            // fanout
            RabbitContext ctx = new RabbitContext().Create(exchange, exchangeType, routingKey);
            var receiver = new LogDirectReceiver(ctx);
            receiver.Run();

        }
    }

    
}
