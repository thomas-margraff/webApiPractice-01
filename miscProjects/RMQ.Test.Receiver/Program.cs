using System;
using RMQLib;

namespace RMQ.Test.Receiver
{
    class Program
    {
        static void Main(string[] args)
        {
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine(string.Format(" {0,2} {1,10} {2,5} {3,5}", "arg0", "arg1", "arg2", "arg3"));
            }
            string exchange = "direct_logs";
            string exchangeType = "direct";
            string routingKey = "info";

            // direct
            // RabbitContext ctx = new RabbitContext().Create("logs", "fanout");
            // var receiver = new LogFanoutReceiver(ctx);

            // fanout
            RabbitContext ctx = new RabbitContext().Create(exchange, exchangeType, routingKey);
            var receiver = new LogDirectReceiver(ctx);
            // receiver.Run();

        }
    }

    
}
