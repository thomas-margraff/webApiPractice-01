using CoronaVirusLib;
using CoronaVirusLib.Configuration;
using Microsoft.Extensions.Configuration;
using RabbitMQLib;
using RabbitMQLib.Messages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using static System.Console;

namespace NewScrapeFileWatcherConsole
{
    // https://developpaper.com/the-correct-way-to-use-rabbitmq-in-net-core/
    public class NewScrapeFileListener : RabbitListener
    {
        private readonly IConfiguration cfg;
        private readonly cvConfig cvConfiguration;
        private readonly ImportScrapeData importData;

        public NewScrapeFileListener(RMQMessage options, 
                                     ImportScrapeData importData,
                                     cvConfig cvConfiguration,
                                     IConfiguration cfg) : base(options)
        {
            this.cfg = cfg;
            this.cvConfiguration = cvConfiguration;
            this.importData = importData;
        }

        public override bool Process(string message)
        {
            // Console.WriteLine($"received {message}");
            importFile(message);
            return true;
        }

        private void importFile(string file)
        {
            WriteLine("{0} New scrape file received", DateTime.Now);
            // sleep 15 seconds - let the file get written...
            Thread.Sleep(15000);
            try
            {
                WriteLine("start import");
                importData.ImportScrapeFile(file);
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
