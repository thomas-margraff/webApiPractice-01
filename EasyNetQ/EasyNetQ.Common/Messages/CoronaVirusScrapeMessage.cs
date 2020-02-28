using System;
using System.Collections.Generic;
using System.Text;
using EasyNetQ;

namespace EasyNetQ.Common.Messages
{
    [Queue("cv.scraper.queue.doscrape", ExchangeName = "cv.scraper.exchange")]
    public class CoronaVirusScrapeMessage: BaseMessage
    {
        public CoronaVirusScrapeMessage()
            :base()
        {

        }

    }
}
