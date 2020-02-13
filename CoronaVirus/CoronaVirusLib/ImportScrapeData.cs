using CoronaVirusDAL;
using CoronaVirusDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoronaVirusLib.Parsers;
using System.IO;
using CoronaVirusDAL.Utilities;
using CoronaVirusLib.Configuration;

namespace CoronaVirusLib
{
    public class ImportScrapeData
    {
        private readonly CvContext ctx;
        private readonly cvParsers parsers;
        private readonly cvConfig cvConfiguration;

        public ImportScrapeData(CvContext dbCtx, cvParsers parsers, cvConfig cvConfiguration)
        {
            this.ctx = dbCtx;
            this.parsers = parsers;
            this.cvConfiguration = cvConfiguration;
        }

        public void ImportScrapeFile(string file)
        {
            Console.WriteLine(new FileInfo(file).Name);
            cvScrapeData data = parsers.ParseJson(file);

            var existScrape = ctx.ScrapeRuns.FirstOrDefault(r => r.Heading.Trim() == data.heading.Trim());
            if (existScrape != null)
            {
                return;
            }
            cvDataUtils.createGeolocationsFromScrapeFile(data);
            cvDataUtils.createCountriesFromScrapeFile(data);
            
            using (var db = new CvContext())
            {
                ScrapeRun scrape = new ScrapeRun()
                {
                    CreateDate = DateTime.Now,
                    Heading = data.heading,
                    ScrapeDate = data.scrapeDate
                };
                db.ScrapeRuns.Add(scrape);
                db.SaveChanges();

                foreach (var geo in data.geoLocations)
                {
                    foreach (var detail in geo.details)
                    {
                        var country = db.Countries.FirstOrDefault(r => r.Name == detail.country);
                        if (country == null && detail.country != "TOTAL")
                        {
                            throw new Exception("country not found: " + detail.country);
                        }
                        if (detail.country == "TOTAL")
                        {
                            continue;
                        }
                        
                        CountryStats stats = new CountryStats()
                        {
                            ScrapeRunId = scrape.Id,
                            CountryId = country.Id,
                            CreateDate = DateTime.Now,
                            CaseCount = int.Parse(detail.cases.Trim().Replace(",", "").Replace("*", "")),
                            DeathCount = int.Parse(detail.deaths.Trim().Replace(",", "").Replace("*", "")),
                            Notes = detail.notes
                        };
                        db.CountryStats.Add(stats);
                    }
                }
                Console.WriteLine();
                try
                {
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public void ImportScrapeFiles()
        {
            var files = parsers.GetJsonFiles();
            files = files.OrderByDescending(r => r).ToList();
            foreach (var file in files)
            {
                ImportScrapeFile(file);
            }
        }

    }
}
