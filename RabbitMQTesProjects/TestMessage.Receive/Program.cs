using System;
using RMQLib;
using static System.Console;

namespace TestMessage.Receive
{
    class Program
    {
        static RabbitContext ctx;
        static void Main(string[] args)
        {
            ctx = new RabbitContext().Create("cv.test.json");
            TestReceiver recv = new TestReceiver(ctx);
        }
    }

    public class TestReceiver: RMQReceiver
    {
        public TestReceiver(RabbitContext ctx) :base(ctx)
        {
            this.Register();
            WriteLine("started import receiver");
            WriteLine(" Press [enter] to exit.");
            ReadLine();
        }

        public override bool Process(string msg)
        {
            WriteLine(msg);
            return true;
        }

    }
}
