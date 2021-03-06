﻿using CoronaVirusDAL;
using CoronaVirusDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoronaVirusLib.Parsers;
using System.IO;
using CoronaVirusDAL.Utilities;
using CoronaVirusLib.Configuration;
using Newtonsoft.Json;

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
            Console.WriteLine(new FileInfo(file).Name);
            ImportScrape(data);
        }

        public void ImportScrapeJson(string json)
        {
            cvScrapeData data = JsonConvert.DeserializeObject<cvScrapeData>(json);
            if (!data.geoLocations.Any())
            {
                Console.WriteLine("no geolocations...");
                return;
            }
            ImportScrape(data);
        }
        
        public void ImportScrape(cvScrapeData data)
        {
            if (data.heading.Contains("XX,XXX"))
            {
                return;
            }
            var existScrape = ctx.ScrapeRuns.FirstOrDefault(r => r.Heading.Trim() == data.heading.Trim());
            if (existScrape != null)
            {
                return;
            }
            Console.WriteLine("updating...");
            cvDataUtils.createGeolocationsFromScrapeFile(data);
            cvDataUtils.createCountriesFromScrapeFile(data);

            ScrapeRun scrape = new ScrapeRun()
            {
                CreateDate = DateTime.Now,
                Heading = data.heading,
                ScrapeDate = data.scrapeDate
            };
            ctx.ScrapeRuns.Add(scrape);
            ctx.SaveChanges();

            foreach (var geo in data.geoLocations)
            {
                foreach (var detail in geo.details)
                {
                    var country = ctx.Countries.FirstOrDefault(r => r.Name == detail.country);
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
                    ctx.CountryStats.Add(stats);
                }
            }
            try
            {
                ctx.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
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

        public vwScrapeRun GetLatestStatsForUnitedStates()
        {
            return GetLatestForCountry("United States");
        }

        public vwScrapeRun GetLatestForCountry(string country)
        {
            return ctx.vwScrapeRuns.Where(r => r.Country == country )
                .OrderByDescending(r => r.ScrapeCreateDate)
                .FirstOrDefault();
        }
    }
}
