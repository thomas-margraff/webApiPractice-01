using CoronaVirusDAL;
using CoronaVirusLib;
using CoronaVirusLib.Configuration;
using CoronaVirusLib.Parsers;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQLib.Messages;
using System;
using System.IO;
using System.Text;
using System.Threading;
using static System.Console;

namespace NewScrapeFileWatcherConsole
{
    public class RabbitMQReceiver
    {
        private readonly IConfiguration cfg;
        private readonly cvConfig cvConfiguration;
        private readonly ImportScrapeData importData;
        private NewScrapeFileListener listener;

        RMQMessage _rmqMsg = new RMQMessage("scrapeFile", "scrapeFile");

        public RabbitMQReceiver(IConfiguration cfg,
                                cvConfig cvConfiguration,
                                ImportScrapeData importData)
        {
            this.cfg = cfg;
            this.cvConfiguration = cvConfiguration;
            this.importData = importData;
        }

        public void Run()
        {
            listener = new NewScrapeFileListener(_rmqMsg, importData, cvConfiguration, cfg);
            listener.Register();
            
            WriteLine("started receiver");
            WriteLine(" Press [enter] to exit.");
            ReadLine();
        }
    }
}
