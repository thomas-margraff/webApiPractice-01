namespace ScrapeServiceWorker.Configuration
{
    public class CoronaVirusScrape
    {
        public bool IsConfigDebug { get; set; }
        public bool IsScheduleDebug { get; set; }
        public string ScrapeUrl { get; set; }
        public bool DryRun { get; set; }
        public string RMQConfigFileDebug { get; set; }
        public string RMQConfigFile { get; set; }
        public CoronaVirusScrape() { }

        public string GetConfigFile()
        {
            if (this.IsConfigDebug)
                return RMQConfigFileDebug;

            return RMQConfigFile;

        }
    }
}
