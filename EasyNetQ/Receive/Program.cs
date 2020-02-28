using System;
using EasyNetQ;
using EasyNetQ.Common.Connection;
using EasyNetQ.Common.Messages;
using static System.Console;

namespace Receive
{
    class Program
    {
        static void Main(string[] args)
        {
            CvScrapeSubscriber receiveMessages = new CvScrapeSubscriber();
            receiveMessages.Receive();
        }
    }
}
