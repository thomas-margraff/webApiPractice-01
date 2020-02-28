using EasyNetQ;
using EasyNetQ.Common.Connection;
using EasyNetQ.Common.Messages;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Send
{
    public class ConsoleApplication
    {
        private readonly IConfiguration configuration;
        //private readonly RabbitBus _bus;
        //private readonly RabbitAdvancedBus _advancedBus;

        public ConsoleApplication(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public void Run()
        {
            using (var bus = RabbitHutch.CreateBus(Connection.Local()))
            {
                DownloadFileMessage downloadFileMessage = new DownloadFileMessage();
                
                CoronaVirusScrapeMessage cvMsg = new CoronaVirusScrapeMessage();
                bus.Send("cv.scraper.queue.doscrape", cvMsg);

                EmailMessage emailMsg = new EmailMessage()
                {
                    Body = "hello from easynetq",
                    Subject = "test rmq"
                };

                emailMsg.Recipients = new List<string>{
                    { "tmargraff@gmail.com" },
                    { "tmargraff.dev@gmail.com" },
                    { "thomas.margraff.dev@gmail.com" },
                };
                bus.Send<EmailMessage>("queue.test", emailMsg);

                //bus.Send<DownloadFileMessage>("queue.test", downloadFileMessage);
                //TextMessage txtMsg = new TextMessage() { Text = "text message" };
                //bus.Send<TextMessage>("queue.test", txtMsg);

            }
        }
    }
}
