using RMQLib.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoronaVirusLib.Messages
{
    public class CoronaVirusScrapeMessage : BaseDataMessage
    {
        public CoronaVirusScrapeMessage()
            : base()
        {
            base.RoutingKey = "doscrape";
        }
    }
}
