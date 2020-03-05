using System;
using System.Collections.Generic;
using System.Text;
using RMQLib;
using static System.Console;

namespace RMQ.Test.Receiver
{
    public class LogFanoutReceiver : RMQLib.Receiver
    {
        private RabbitContext _ctx;

        public LogFanoutReceiver(RabbitContext ctx) :
            base(ctx)
        {
            _ctx = ctx;
        }

        public void Run()
        {
            base.Register();

            WriteLine(" Receiver... Press [enter] to exit.");
            ReadLine();
        }

        public override bool Process(string json)
        {
            WriteLine(json);
            return true;
        }
    }
}
