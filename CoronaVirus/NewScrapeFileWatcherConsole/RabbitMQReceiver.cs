using CoronaVirusDAL;
using CoronaVirusLib;
using CoronaVirusLib.Configuration;
using CoronaVirusLib.Parsers;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.IO;
using System.Text;
using System.Threading;
using static System.Console;

namespace NewScrapeFileWatcherConsole
{
    public class RabbitMQReceiver
    {
        private readonly CvContext ctx;
        private readonly IConfiguration cfg;
        private readonly ImportScrapeData importData;
        private readonly cvParsers cvParsers;
        private readonly cvConfig cvConfiguration;

        public RabbitMQReceiver(CvContext ctx,
                                IConfiguration cfg,
                                ImportScrapeData importData,
                                cvParsers cvParsers,
                                cvConfig cvConfiguration)
        {
            this.ctx = ctx;
            this.cfg = cfg;
            this.importData = importData;
            this.cvParsers = cvParsers;
            this.cvConfiguration = cvConfiguration;
        }


        public void Run()
        {
            this.Receive();
        }
        public void Receive()
        {
            WriteLine("started receiver");

            var factory = new ConnectionFactory() 
            { 
                HostName = "gull-01.rmq.cloudamqp.com",
                VirtualHost = "noekmbda",
                UserName = "noekmbda",
                Password = "jtKtg3LU2uEQOCJRpEgMhdSqf2N7S_Cv"
            };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "scrapeFile", durable: false, exclusive: false, autoDelete: false, arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    // WriteLine(" [x] Received {0}", message);
                    importFile(message);
                };
                channel.BasicConsume(queue: "scrapeFile", autoAck: true, consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
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
