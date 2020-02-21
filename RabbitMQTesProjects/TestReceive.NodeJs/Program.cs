using System;
using TestLib;
using static System.Console;

namespace TestReceive.NodeJs
{
    class Program
    {
        static void Main(string[] args)
        {
            RabbitContext ctx = new RabbitContext().Create("cv.scraper.json");
            var receiverJson = new cvMessageReceive(ctx);

            WriteLine("started receiver");
            WriteLine(" Press [enter] to exit.");
            ReadLine();

        }
    }
}
