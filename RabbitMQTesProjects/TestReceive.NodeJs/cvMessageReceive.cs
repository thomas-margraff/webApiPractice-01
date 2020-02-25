using RMQLib;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Console;

namespace TestReceive
{
    public class cvMessageReceive : Receiver
    {
        public cvMessageReceive(RabbitContext ctx) : base(ctx) 
        {
            this.Register();
        }

        public override bool Process(string message)
        {
            // Console.WriteLine($"received {message}");
            WriteLine(message);
            return true;
        }

    }
}
