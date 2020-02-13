using CoronaVirusDAL;
using CoronaVirusDAL.Entities;
using CoronaVirusLib.Configuration;
using CoronaVirusLib.Parsers;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cvDBTester
{
    public class ScrapeRunsTest
    {
        private readonly CvContext ctx;
        private readonly IConfiguration cfg;
        private readonly cvParsers parsers;
        private readonly cvConfig cvConfiguration;

        public ScrapeRunsTest(CvContext ctx,
                              IConfiguration cfg,
                              cvParsers parsers,
                              cvConfig cvConfiguration)
        {
            this.ctx = ctx;
            this.cfg = cfg;
            this.parsers = parsers;
            this.cvConfiguration = cvConfiguration;
        }

        public void ExistsVsNew()
        {
            var files = parsers.GetJsonFiles();
            int existCount = 0; // 10
            int newCount = 0;   // 32

            foreach (var file in files)
            {
                cvScrapeData data = parsers.ParseJson(file);
                var existScrape = ctx.ScrapeRuns.FirstOrDefault(r => r.Heading == data.heading);
                if (existScrape != null)
                {
                    existCount++;
                }
                else
                {
                    newCount++;
                }
            }
            Console.WriteLine("exists: {0}", existCount);
            Console.WriteLine("new   : {0}", newCount);
        }

        public void ParseLocalFile()
        {
            this.parsers.ParseHtmlFile();
        }
    }
}
