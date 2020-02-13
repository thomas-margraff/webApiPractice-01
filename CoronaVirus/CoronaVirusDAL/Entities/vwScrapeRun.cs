using System;
using System.Collections.Generic;
using System.Text;

namespace CoronaVirusDAL.Entities
{
    public partial class vwScrapeRun
    {
        public string Heading {get;set;}
        public DateTime ScrapeDate {get;set;}
        public string GeoLocation {get;set;}
        public string Country {get;set;}
        public int Cases {get;set;}
        public int Deaths {get;set;}
        public string Notes {get;set;}
        public int ScrapeRunId {get;set;}
        public int GeoLocationId {get;set;}
        public int CountryId {get;set;}
        public int CountryStatsId {get;set;}

        public DateTime ScrapeCreateDate {get;set;}

    }
}
