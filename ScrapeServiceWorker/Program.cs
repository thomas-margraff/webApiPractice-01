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
        public static void Main(string[] args)
        {
            IHost host = CreateHostBuilder(args).Build();
            host.Services.UseScheduler(scheduler =>
            {
                Console.WriteLine("Scrape job started...");

                scheduler
                    .Schedule<ScraperInvocable>()
                    //.EveryFifteenSeconds();
                    //.Cron("* * * * *")
                    //.Cron("2 0 0 0 0");
                    .DailyAtHour(23);
            });
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

                    //services.AddHostedService<Worker>();
                });
    }
}
