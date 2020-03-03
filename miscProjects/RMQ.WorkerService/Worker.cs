using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CoronaVirusLib;
using CoronaVirusLib.Configuration;
using CoronaVirusLib.Parsers;
using CoronaVirusLib.Receivers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RMQLib;

namespace RMQ.WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly RabbitContext _ctxCloudAmqp;
        private readonly RabbitContext _ctxLocal;
        private readonly cvConfig _cvConfig;
        private readonly cvParsers _cvParsers;
        private readonly ILogger<Worker> _logger;
        private readonly ImportScrapeData _importScrapeData;



        public Worker(ILogger<Worker> logger, 
                      RabbitContext ctxCloudAmqp, 
                      RabbitContext ctxLocal)
        {
            this._ctxCloudAmqp = ctxCloudAmqp;
            this._ctxLocal = ctxLocal;
            this._logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(500, stoppingToken);
            var apiReceiver = new apiDataReceiver(_ctxLocal);

            // _scrapeImportReceiver = new ScrapeImportReceiver(_ctxCloudAmqp, _importScrapeData, _cvConfig);

            //while (!stoppingToken.IsCancellationRequested)
            //{
            //    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            //    await Task.Delay(1000, stoppingToken);
            //}
        }
    }
}
