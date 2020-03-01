namespace ScrapeServiceWorker.Configuration
{
    public class CalendarScrape
    {
        public bool IsDebug { get; set; }
        public string ScrapeUrl { get; set; }
        public string ScrapeDebugUrl { get; set; }
        public bool BulkUpdate { get; set; }
        public bool DryRun { get; set; }

        public CalendarScrape() { }
    }
}
