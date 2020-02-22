using RMQLib;
using System;

namespace TestContextLoader
{
    class Program
    {
        static void Main(string[] args)
        {
            // var ctx = new RabbitContext().Create("cv.scraper.json");
            var ctx = new RabbitContext().Create("test.json");
            var sender = new Sender(ctx);
            sender.Send("test");
        }
    }
}
