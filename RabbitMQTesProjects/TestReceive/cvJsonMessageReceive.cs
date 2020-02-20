using System;
using System.Collections.Generic;
using System.Text;
using TestLib;
using static System.Console;

namespace TestReceive
{
    public class cvJsonMessageReceive : Receiver
    {
        public cvJsonMessageReceive(RabbitContext ctx) : base(ctx) 
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
