using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoronaVirusDAL.Entities
{
    [Table("ScrapeRuns") ]  // , Schema = "dbo.ScrapeRuns")]
    public class ScrapeRun
    {
        public int Id { get; set; }
        public string Heading { get; set; }
        public DateTime ScrapeDate { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? ModifyDate { get; set; }
        public List<CountryStats> CountryStats { get; set; }
    }

    public class GeoLocation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? ModifyDate { get; set; }
        public List<Country> Countries { get; set; }

        
    }

    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? ModifyDate { get; set; }

        // [ForeignKey("GeoLocationId")]
        public GeoLocation GeoLocation { get; set; }
        public int GeoLocationId { get; set; }

    }

    public class CountryStats
    {
        public int Id { get; set; }
        public int CaseCount { get; set; }
        public int DeathCount { get; set; }
        public string Notes { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? ModifyDate { get; set; }

        // [ForeignKey("ScrapeRunId")]
        public ScrapeRun ScrapeRun { get; set; }
        public int ScrapeRunId { get; set; }

        // [ForeignKey("CountryId")]
        public Country Country { get; set; }
        public int CountryId { get; set; }

    }
}
