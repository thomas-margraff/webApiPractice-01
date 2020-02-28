using EasyNetQ;
using EasyNetQ.Common.Connection;
using EasyNetQ.Common.Messages;
using EmailLib;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Console;

namespace Receive
{
    public class CvScrapeSubscriber
    {
        public void Receive()
        {
            using (var bus = RabbitHutch.CreateBus(Connection.Local()))
            {
                bus.Receive<CoronaVirusScrapeMessage>("cv.scraper.queue.doscrape", message => HandleCvMessage(message));

                Console.WriteLine("Listening for messages. Hit <return> to quit.");
                ReadLine();
            }
        }


        private void HandleCvMessage(CoronaVirusScrapeMessage emailMsg)
        {
            try
            {
                int i = 2;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
