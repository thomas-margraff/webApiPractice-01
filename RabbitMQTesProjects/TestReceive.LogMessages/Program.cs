using System;
using TestLib;
using static System.Console;

namespace TestReceive.LogMessages
{
    class Program
    {
        static void Main(string[] args)
        {
            RabbitContext ctxLog = new RabbitContext().Create("logging.json");
            var receiverLog = new LogMessageReceive(ctxLog);

            WriteLine("started receiver");
            WriteLine(" Press [enter] to exit.");
            ReadLine();

        }
    }
}
