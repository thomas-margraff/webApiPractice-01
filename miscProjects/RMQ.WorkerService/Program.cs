using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoronaVirusDAL;
using CoronaVirusLib;
using CoronaVirusLib.Configuration;
using CoronaVirusLib.Parsers;
using CoronaVirusLib.Receivers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RMQLib;

namespace RMQ.WorkerService
{
    public class Program
    {
        static IConfiguration Configuration { get; set; }

        public static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder().AddJsonFile("appSettings.json");
            Program.Configuration = builder.Build();
            CreateHostBuilder(args).Build().RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();
                    //services.AddDbContext<CvContext>(options =>
                    //{
                    //    options.UseSqlServer(Configuration.GetConnectionString("cvConnectionString"));
                    //});

                    // rabbitmq
                    var ctxCloudAmqp = new RabbitContext().Create("cv.scraper.json");
                    var ctxLocal = new RabbitContext().Create("cv.localhost.json");
                    services.AddSingleton(ctxCloudAmqp);
                    services.AddSingleton(ctxLocal);
                    services.AddSingleton<IConfiguration>(Program.Configuration);
                    
                    // services.AddTransient<ScrapeImportReceiver>();
                    //services.AddTransient<cvConfig>();
                    //services.AddTransient<cvParsers>();
                    //services.AddTransient<ImportScrapeData>();
                });

    }
}
