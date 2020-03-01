using CoronaVirusLib;
using CoronaVirusLib.Configuration;
using CoronaVirusLib.Messages;
using RMQLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using static System.Console;

namespace NewScrapeFileWatcherConsole
{
    public class ImportRunner
    {
        private readonly cvConfig _cvConfig;
        private readonly ImportScrapeData _importData;

        public ImportRunner(ImportScrapeData importData, cvConfig cvConfig)
        {
            _cvConfig = cvConfig;
            this._importData = importData;
        }

        public void Run()
        {
            RabbitContext ctx = new RabbitContext().Create("cv.scraper.json");
            var receiverJson = new ScrapeFileWatcherReceive(ctx, _importData, _cvConfig);

            WriteLine("started import receiver");
            WriteLine(" Press [enter] to exit.");
            ReadLine();
        }
    }

    public class ScrapeFileWatcherReceive : Receiver
    {
        private readonly cvConfig _cvConfig;
        private readonly ImportScrapeData _importData;

        public ScrapeFileWatcherReceive(RabbitContext ctx,
                                    ImportScrapeData importData,
                                    cvConfig cvConfig) : base(ctx)
        {
            this._cvConfig = cvConfig;
            this._importData = importData;
            var scrapeDataMessage = new CoronaVirusScrapeDataMessage();

            ctx.Binder.RoutingKey = scrapeDataMessage.RoutingKey;
            this.Register();
        }

        public override bool Process(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return true;

            importJson(json);
            return true;
        }

        private void importJson(string json)
        {
            string dashes = new String('=', 65);
            WriteLine(dashes);
            ForegroundColor = ConsoleColor.Green;
            WriteLine("{0} New json data received", DateTime.Now);

            try
            {
                ForegroundColor = ConsoleColor.White;
                WriteLine("start import");
                _importData.ImportScrapeJson(json);
                var rec = _importData.GetLatestStatsForUnitedStates();
                WriteLine(rec.Heading);
                WriteLine("  Latest United States Stats");
                WriteLine("    Cases : {0}", rec.Cases);
                WriteLine("    Deaths: {0}", rec.Deaths);
                WriteLine("    Notes : {0}", rec.Notes);
                WriteLine("end import");
                WriteLine("");
            }
            catch (Exception ex)
            {
                WriteLine(ex.Message);
                throw ex;
            }
        }

        //public override bool Process(string file)
        //{
        //    if (string.IsNullOrWhiteSpace(file))
        //        return true;

        //    WriteLine("new message {0}", file);
        //    importFile(file);
        //    return true;
        //}


        private void importFile(string file)
        {
            WriteLine("{0} New scrape file received", DateTime.Now);

            // sleep 15 seconds - let the file get written...
            Thread.Sleep(15000);
            if (!File.Exists(file))
            {
                WriteLine("{0} doesn't exist", file);
                return;
            }
            try
            {
                WriteLine("start import");
                _importData.ImportScrapeFile(file);
                WriteLine("end import");
                WriteLine("");
            }
            catch (Exception ex)
            {
                WriteLine(ex.Message);
                throw ex;
            }
        }

    }
}
