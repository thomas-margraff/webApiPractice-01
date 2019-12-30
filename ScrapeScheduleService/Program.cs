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
                // Easy peasy
                scheduler
                    .Schedule<NestScraper>()
                    .EveryFiveSeconds()
                    .Weekday();
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

