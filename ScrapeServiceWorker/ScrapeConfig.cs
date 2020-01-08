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
        public string ScrapeUrl { get; set; }
        public bool BulkUpdate { get; set; }
    }
}
