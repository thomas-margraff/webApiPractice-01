using CoronaVirusDAL;
using CoronaVirusDAL.Entities;
using CoronaVirusLib;
using CoronaVirusLib.Configuration;
using CoronaVirusLib.Parsers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base.Enums;
using TableDependency.SqlClient.Base.EventArgs;

namespace cvDBTester
{
    public class DBTester
    {
        private readonly CvContext ctx;
        private readonly IConfiguration cfg;
        private readonly cvParsers parsers;
        private readonly ImportScrapeData importer;
        private readonly cvConfig cvConfiguration;
        private ScrapeRunsTest runtest;

        public DBTester(CvContext ctx,
                        IConfiguration cfg,
                        cvParsers parsers,
                        ImportScrapeData importer,
                        cvConfig cvConfiguration)
        {
            this.ctx = ctx;
            this.cfg = cfg;
            this.parsers = parsers;
            this.importer = importer;
            this.cvConfiguration = cvConfiguration;

            runtest = new ScrapeRunsTest(ctx, cfg, parsers, cvConfiguration);
        
        }

        public void Run()
        {
            // tableChangeNotifications();
            // testScrapeRuns();
            doScrapeFileImport();
            // testIncludes();
            // testView();
            // this.runtest.ExistsVsNew();
            // ParseLocalFile();
        }

        void tableChangeNotifications()
        {
            SQLNotifications notifications = new SQLNotifications(this.cvConfiguration);
            notifications.Run();
        }

        void testScrapeRuns()
        {
            ScrapeRunsTest st = new ScrapeRunsTest(ctx, cfg, parsers, cvConfiguration);
            st.ExistsVsNew();
        }

        void doScrapeFileImport()
        {
            importer.ImportScrapeFiles();
        }
            
        void testIncludes()
        {
            var cs = ctx.CountryStats
                .Where(r => r.ScrapeRun.Id == 120)
                .Include(c => c.Country)
                .ThenInclude(c => c.GeoLocation)
                .Include(s => s.ScrapeRun)
                .FirstOrDefault();

            var us = ctx.CountryStats
                .Include(c => c.Country)
                .ThenInclude(c => c.GeoLocation)
                .Include(s => s.ScrapeRun)
                .Where(r => r.Country.Name.Contains("united states")
                ).ToList();
        }

        void testView()
        {
            var recs = ctx.vwScrapeRuns.ToList();
            int x = 2;
        }

        void ParseLocalFile()
        {
            this.parsers.ParseHtmlFile();
        }

    }
}
