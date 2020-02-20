using System;
using TestLib;
using TestLib.Messages;
using static System.Console;

namespace TestReceive
{
    class Program
    {
        static void Main(string[] args)
        {
            RabbitContext ctx = new RabbitContext().Create("cv.scraper.json");
            var receiverJson = new cvJsonMessageReceive(ctx);

            WriteLine("started receiver");
            WriteLine(" Press [enter] to exit.");
            ReadLine();

        }
    }

    public class TestListener : Receiver
    {
        public TestListener(RabbitContext ctx) : base(ctx) { }
        public override bool Process(string message)
        {
            // Console.WriteLine($"received {message}");
            WriteLine(message);
            return true;
        }

    }
}
