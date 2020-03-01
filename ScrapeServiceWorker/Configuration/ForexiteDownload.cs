namespace ScrapeServiceWorker.Configuration
{
    public class ForexiteDownload
    {
        public bool IsDebug { get; set; }
        public string ForexiteUrl { get; set; }
        public string ForexiteArchivePath { get; set; }
        public bool DryRun { get; set; }

        public ForexiteDownload() { }
    }
}
