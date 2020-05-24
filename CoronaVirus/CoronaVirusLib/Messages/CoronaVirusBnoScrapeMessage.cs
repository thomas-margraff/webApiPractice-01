using CoronaVirusLib.Models;
using RMQLib.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoronaVirusLib.Messages
{
    public class CoronaVirusBnoScrapeMessage : BaseDataMessage
    {
        public List<BnoRowData> ScrapeRowData { get; set; }
        public string PayloadJson { get; set; }

        public CoronaVirusBnoScrapeMessage()
        {
            base.RoutingQueueName = "cv.messages.queue";
            base.RoutingKey = "cvBnoScrape";
        }
    }
}
