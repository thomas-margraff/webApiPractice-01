using RMQLib;
using RMQLib.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestSend
{
    public class RMQScrapeServiceWorker
    {
        private RabbitContext ctx;
        private ScrapeServiceWorkerSend _sender;
        private ScrapeServiceWorkerReceive _receiver;
        public RMQScrapeServiceWorker()
        {
            ctx = new RabbitContext().Create("cv.scraper.json");
            _receiver = new ScrapeServiceWorkerReceive(ctx);
            _sender = new ScrapeServiceWorkerSend(ctx);
        }

        public void Send(string msg)
        {
            _sender.Send(msg);
        }
    }
}
