using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DAL_SqlServer.Models;
using Coravel;
using DAL_SqlServer.Repository;
using DAL_SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ScrapeServiceWorker
{
    public class Program
    {
        static IConfiguration Configuration { get; set; }

        public static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder().AddJsonFile("appSettings.json");
            Program.Configuration = builder.Build();

            IHost host = CreateHostBuilder(args).Build();

            host.Services.UseScheduler(scheduler =>
            {
                Console.WriteLine("Scrape job started...");
                // var hrCal = DateTime.Now.ToUniversalTime().TimeOfDay.Hours;
                var hrPrices = new DateTime(2020, 1, 13, 23, 59, 0).ToUniversalTime().TimeOfDay.Hours;
                var hrCal = hrPrices + 2;
                scheduler.Schedule<PriceDownloaderInvocable>().DailyAt(hrCal, 0); ;
                scheduler.Schedule<ScraperInvocable>().DailyAt(hrCal, 21);
                // scheduler.Schedule<ScraperInvocable>().EveryFifteenSeconds();

            }).OnError((exception) =>
                {
                    Console.WriteLine("It's broken!");
                    Console.WriteLine(exception.Message);
                }
            );
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddDbContext<ntpContext>(cfg =>
                    {
                        cfg.UseSqlServer(hostContext.Configuration.GetConnectionString("ntpConnectionString"));
                    });

                    services.AddScoped<IIndicatorDataRepository, IndicatorDataRepository<ntpContext>>();
                    services.AddScheduler();
                    services.AddTransient<ScraperInvocable>();
                    services.AddTransient<PriceDownloaderInvocable>();
                    services.AddSingleton<IConfiguration>(Program.Configuration);

                    var scrapeConfig = new ScrapeConfig();
                    Configuration.GetSection("ScrapeConfiguration").Bind(scrapeConfig);
                    services.AddSingleton(scrapeConfig);

                    //services.AddHostedService<Worker>();
                });
    }
}
