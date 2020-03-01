using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScrapeServiceWorker.Configuration
{
    public class ScrapeConfig
    {
        public bool IsDebug { get; set; }
        public int StartHour { get; set; }
        public int StartMinute { get; set; }
        public string Note { get; set; }
        public int CalendarOffsetHours { get; set; }
        public bool DryRun { get; set; }

        public CalendarScrape CalendarScrape { get; set; }
        public ForexiteDownload ForexiteDownload { get; set; }
        public CoronaVirusScrape CoronaVirusScrape { get; set; }
        public CoronaVirusApiTracker CoronaVirusApiTracker { get; set; }

        public ScrapeConfig() { }

    }
}
