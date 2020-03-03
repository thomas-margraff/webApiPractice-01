using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CoronaVirusDAL;
using CoronaVirusLib;
using CoronaVirusLib.Configuration;
using CoronaVirusLib.Parsers;
using CoronaVirusLib.Receivers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RMQLib;

namespace RMQ.WorkerService.Consumer
{
    public class ApiTrackerConsumer : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly RabbitContext _ctxCloudAmqp;
        private readonly RabbitContext _ctxLocal;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private apiDataReceiver _consumer;

        public ApiTrackerConsumer(ILogger<Worker> logger,
                                  RabbitContext ctxCloudAmqp,
                                  RabbitContext ctxLocal,
                                  IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _ctxCloudAmqp = ctxCloudAmqp;
            _ctxLocal = ctxLocal;
            _serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            return base.StartAsync(cancellationToken);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var cfg = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                var cvCfg = scope.ServiceProvider.GetRequiredService<cvConfig>();
                var cvp = scope.ServiceProvider.GetRequiredService<cvParsers>();
                var dbCtx = scope.ServiceProvider.GetRequiredService<CvContext>();
                var imp = new ImportScrapeData(dbCtx, cvp, cvCfg);
                _consumer = new apiDataReceiver(_ctxLocal);
                _consumer.Run();

                await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
            }
        }
    }
}
