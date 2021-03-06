﻿using System;
using RMQLib;
using static System.Console;

namespace TestMessage.Send
{
    class Program
    {
        static RabbitContext ctx;

        static void Main(string[] args)
        {
            ctx = new RabbitContext().Create("cv.test.json");
            RmqSender sender = new RmqSender(ctx);

            while (true)
            {
                WriteLine("enter a msg to send");
                string msg = ReadLine();
                sender.Send(msg);
            }
        }
    }
}
