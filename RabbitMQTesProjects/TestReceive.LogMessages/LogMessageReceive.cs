﻿using RMQLib;
using System;
using System.Collections.Generic;
using System.Text;

using static System.Console;

namespace TestReceive.LogMessages
{
    public class LogMessageReceive : Receiver
    {
        public LogMessageReceive(RabbitContext ctx) : base(ctx) 
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
