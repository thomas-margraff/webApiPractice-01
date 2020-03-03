using CoronaVirusLib.Messages;
using Newtonsoft.Json;
using RMQLib;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Console;

namespace RMQ.Console.Listener
{
    public class MessageListener : Receiver
    {
        public MessageListener(RabbitContext ctx):
            base(ctx)
        {
        }

        public void Run()
        {
            base.Register(new CoronaVirusApiTrackerMessage());

            WriteLine("started import receiver");
            WriteLine(" Press [enter] to exit.");
            ReadLine();
        }
        public override bool Process(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return true;

            var msg = JsonConvert.DeserializeObject<CoronaVirusApiTrackerMessage>(json);

            return true;
        }

    }
}
