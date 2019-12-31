using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Coravel;

namespace ScrapeScheduleService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IHost host = CreateHostBuilder(args).Build();
            host.Services.UseScheduler(scheduler =>
            {
                Console.WriteLine("Scrape job started...");
                Console.WriteLine("First Scrape job starts in 30 minutes: " + DateTime.Now.AddMinutes(60));

                // Easy peasy
                scheduler
                    .Schedule<NestScraper>()
                    .HourlyAt(54);
            });
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>

        Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddScheduler();
                    // Add this
                    services.AddTransient<NestScraper>();
                });
    };
}

