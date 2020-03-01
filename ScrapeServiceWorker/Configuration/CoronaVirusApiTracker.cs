using System;
using System.Collections.Generic;
using System.Text;

namespace ScrapeServiceWorker.Configuration
{
    public class CoronaVirusApiTracker
    {
        public bool IsDebug { get; set; }
        public string RMQConfigFile { get; set; } = "cv.localhost.json";

        public CoronaVirusApiTracker() { }

        public string GetConfigFile()
        {
            return this.RMQConfigFile;
        }
    }


}
