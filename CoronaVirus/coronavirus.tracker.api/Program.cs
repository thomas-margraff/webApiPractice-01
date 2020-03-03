using Newtonsoft.Json;
using RMQLib;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using static System.Console;


namespace coronavirus.tracker.api
{
    class Program
    {
        static void Main(string[] args)
        {
            RabbitContext ctx = new RabbitContext().Create("cv.scraper.json");
            var receiver = new apiDataReceiver(ctx);
            receiver.Run();
        }
	}
}
