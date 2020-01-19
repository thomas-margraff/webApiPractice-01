using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScrapeServiceWorker
{
    public class ScrapeConfig
    {
        public ScrapeConfig()
        {
        }

        public bool IsDebug { get; set; }
        public string ScrapeUrl { get; set; }
        public string ScrapeDebugUrl { get; set; }
        public bool BulkUpdate { get; set; }
        public string ForexiteUrl { get; set; }
        public int StartHour { get; set; }
        public int StartMinute { get; set; }
        public string Note { get; set; }
        public int CalendarOffsetHours { get; set; }
        public bool DryRun { get; set; }

    }
}
