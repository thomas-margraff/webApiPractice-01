using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoronaVirusDAL.Entities
{
    public class VirusStats
    {
        [Key]
        public int Id { get; set; }
        public DateTime ScrapeDateTime { get; set; }
        public string Geolocation { get; set; }
        public string Country { get; set; }
        public string Cases { get; set; }
        public int Deaths { get; set; }
        public string Notes { get; set; }

    }
}
