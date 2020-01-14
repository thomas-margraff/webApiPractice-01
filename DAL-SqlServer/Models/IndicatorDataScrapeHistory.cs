using System;
using System.Collections.Generic;
using System.Text;

namespace DAL_SqlServer.Models
{
    public class IndicatorDataScrapeHistory
    {
        public int Id { get; set; }
        public DateTime ScrapeDate { get; set; }
        public int RecordCount { get; set; }
    }
}
