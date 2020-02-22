using RMQLib;
using RMQLib.Messages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ScrapeServiceWorker.RMQ
{
    public class cvJsonMessage
    {
        private RabbitContext ctx;
        private ScrapeServiceWorkerSend _sender;
        public cvJsonMessage(CoronaVirusScrape config)
        {
            string cfgFile = config.GetConfigFile();
            ctx = new RabbitContext().Create(cfgFile);
            _sender = new ScrapeServiceWorkerSend(ctx);
        }

        public void Publish(string msg)
        {
            _sender.Send(msg);
        }

        public void Publish(RabbitMessage msg)
        {
            _sender.Send(msg);
        }
    }
}
