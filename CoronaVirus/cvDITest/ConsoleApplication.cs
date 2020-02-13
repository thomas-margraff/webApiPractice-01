using CoronaVirusDAL;
using CoronaVirusLib.Configuration;
using CoronaVirusLib.Parsers;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cvDITest
{
    public class ConsoleApplication
    {
        private readonly CvContext ctx;
        private readonly IConfiguration cfg;
        private readonly cvParsers parsers;
        private readonly cvConfig cvConfiguration;

        public ConsoleApplication(CvContext ctx, 
                                  IConfiguration cfg,
                                  cvParsers parsers,
                                  cvConfig cvConfiguration)
        {
            this.ctx = ctx;
            this.cfg = cfg;
            this.parsers = parsers;
            this.cvConfiguration = cvConfiguration;
        }

        public void Run()
        {
            var files = parsers.GetJsonFiles();
            var x = cvConfiguration.JsonDir();
            x = cvConfiguration.GetValue("csvDir");
            var y = cvConfiguration.GetValue("csvDir");

            var jsonDir = cfg.GetValue<string>("jsonDir");
            var csvDir = cfg.GetValue<string>("csvDir");

            var recs = ctx.ScrapeRuns.ToList();

        }
    }
}
