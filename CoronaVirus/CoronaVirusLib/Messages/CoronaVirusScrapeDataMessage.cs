using RMQLib.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoronaVirusLib.Messages
{
    public class CoronaVirusScrapeDataMessage : BaseDataMessage
    {
        public CoronaVirusScrapeDataMessage()
            :base()
        {
            base.RoutingKey = "importscrape";
        }
    }
}
