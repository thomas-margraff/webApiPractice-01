using System;
using System.Collections.Generic;
using System.Text;

namespace CoronaVirusDAL.Entities
{
    public class cvScrapeData
    {
        public DateTime scrapeDate { get; set; }
        public string heading { get; set; }
        public List<cvModel> geoLocations { get; set; }

        public cvScrapeData()
        {
            this.scrapeDate = DateTime.Now;
            this.geoLocations = new List<cvModel>();
        }
    }

    public class cvModel
    {
        public string name { get; set;}
        public List<cvDetail> details { get; set; }
        public cvModel()
        {
            this.details = new List<cvDetail>();
        }
    }

    public class cvDetail
    {
        public string country { get; set; }
        public string cases { get; set; }
        public string deaths { get; set; }
        public string notes { get; set; }
    }
}
