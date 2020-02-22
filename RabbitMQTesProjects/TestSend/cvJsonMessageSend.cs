using RMQLib;
using RMQLib.Messages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TestSend
{
    public class cvJsonMessageSend
    {
        private RabbitContext ctx;

        public cvJsonMessageSend()
        {
            ctx = new RabbitContext().Create("cv.scraper.json");
            ctx.Binder.RoutingKey = "doscrape";
        }

        public void Publish(string msg)
        {
            var sender = new Sender(ctx);
            sender.Send(msg);
        }

        public void Publish(RabbitMessage msg)
        {
            var sender = new Sender(ctx);
            sender.Send(msg);
        }

        public void Publish()
        {
            var sender = new Sender(ctx);
            var fname = @"C:\devApps\typescriptscraper\coronavirus\data\json\2020.02.09-1601-bno.json";
            var json = File.ReadAllText(fname);
            var rmsg = new RabbitMessage("cvScrapeJson", json);
            sender.Send(rmsg);
        }
    }
}
