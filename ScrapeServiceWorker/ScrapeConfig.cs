using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScrapeServiceWorker
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

        public ScrapeConfig() { }
    }

    public class CalendarScrape
    {
        public bool IsDebug { get; set; }
        public string ScrapeUrl { get; set; }
        public string ScrapeDebugUrl { get; set; }
        public bool BulkUpdate { get; set; }
        public bool DryRun { get; set; }

        public CalendarScrape() { }
    }
    public class ForexiteDownload
    {
        public bool IsDebug { get; set; }
        public string ForexiteUrl { get; set; }
        public string ForexiteArchivePath { get; set; }
        public bool DryRun { get; set; }

        public ForexiteDownload() { }
    }

    public class CoronaVirusScrape
    {
        public bool IsDebug { get; set; }
        public string ScrapeUrl { get; set; }
        public bool DryRun { get; set; }
        public string RMQConfigFileDebug { get; set; }
        public string RMQConfigFile { get; set; }
        public CoronaVirusScrape() { }

        public string GetConfigFile()
        {
            if (this.IsDebug)
                return RMQConfigFileDebug;

            return RMQConfigFile;

        }
    }
}
