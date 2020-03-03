using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoronaVirusLib;
using CoronaVirusLib.Configuration;
using CoronaVirusDAL;
using CoronaVirusLib.Parsers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RMQLib;

namespace RMQ.WorkerService.Consumer
{
    public class Program
    {
        static IConfiguration Configuration { get; set; }

        public static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder().AddJsonFile("appSettings.json");
            Program.Configuration = builder.Build();

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    // services.AddHostedService<Worker>();

                    services.AddHostedService<ApiTrackerConsumer>();
                    services.AddSingleton<IConfiguration>(Program.Configuration);

                    services.AddDbContext<CvContext>(cfg =>
                    {
                        cfg.UseSqlServer(hostContext.Configuration.GetConnectionString("cvConnectionString"));
                    });

                    var cvCfg = new cvConfig(Program.Configuration);
                    services.AddSingleton<cvConfig>(cvCfg);

                    var cvp = new cvParsers(cvCfg);
                    services.AddSingleton<cvParsers>(cvp);

                    // rabbitmq
                    var ctxCloudAmqp = new RabbitContext().Create("cv.scraper.json");
                    var ctxLocal = new RabbitContext().Create("cv.localhost.json");
                    services.AddSingleton(ctxCloudAmqp);
                    services.AddSingleton(ctxLocal);

                });
    }
}
