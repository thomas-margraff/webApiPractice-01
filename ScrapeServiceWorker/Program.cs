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
using ScrapeServiceWorker.Configuration;
using static System.Console;
using ScrapeServiceWorker.CoronaVirusApiTracker;
using ScrapeServiceWorker.Forexite;
using ScrapeServiceWorker.CalendarScraper;
using ScrapeServiceWorker.CoronaVirusScraper;

namespace ScrapeServiceWorker
{
    public class Program
    {
        static ScrapeConfig scrapeConfig;
        static ScrapeCache scrapeCache;
        static IConfiguration Configuration { get; set; }

        public static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder().AddJsonFile("appSettings.json");
            Program.Configuration = builder.Build();

            IHost host = CreateHostBuilder(args).Build();

            host.Services.UseScheduler(scheduler =>
            {
                ForegroundColor = ConsoleColor.Green;
                WriteLine("Scrape job started...");
                WriteLine(scrapeConfig.Note);
                WriteLine("");
                ForegroundColor = ConsoleColor.White;

                var hrStart = scrapeConfig.StartHour;
                var minStart = scrapeConfig.StartMinute;
                var hrStartCalendarOffsetHours = scrapeConfig.CalendarOffsetHours;

                if (!scrapeConfig.CoronaVirusApiTracker.IsDebug)
                    scheduler.Schedule<cvApiTrackerInvocable>().EveryFifteenMinutes();
                else
                    scheduler.Schedule<cvApiTrackerInvocable>().EveryFifteenSeconds();

                if (!scrapeConfig.ForexiteDownload.IsDebug)
                    scheduler.Schedule<PriceDownloaderInvocable>().DailyAt(hrStart, minStart);
                else
                    scheduler.Schedule<PriceDownloaderInvocable>().EveryFifteenSeconds();

                if (!scrapeConfig.CalendarScrape.IsDebug)
                    scheduler.Schedule<ScraperInvocable>().DailyAt(hrStartCalendarOffsetHours, minStart);
                else
                    scheduler.Schedule<CVScraperInvocable>().EveryFifteenSeconds();

                if (!scrapeConfig.CoronaVirusScrape.IsScheduleDebug)
                    scheduler.Schedule<CVScraperInvocable>().EveryThirtyMinutes();
                else
                    scheduler.Schedule<CVScraperInvocable>().EveryFifteenSeconds();

                #region cron docs
                //  run every two hours and 11 minutes
                // scheduler.Schedule<CVScraperInvocable>().Cron("11 */2 * * *");

                /* 
                          1 2 3 4 5                  
                    Cron("* * * * *)

                    1	Minute	0 to 59, or * (no specific value)
                    2	Hour	0 to 23, or * for any value. All times UTC.
                    3	Day of the month	1 to 31, or * (no specific value)
                    4	Month	1 to 12, or * (no specific value)
                    5	Day of the week	0 to 7 (0 and 7 both represent Sunday), or * (no specific value)

                    Examples: Cron time string format
                    https://support.acquia.com/hc/en-us/articles/360004224494-Cron-time-string-format

                    https://support.acquia.com/hc/en-us/articles/360004224494-Cron-time-string-format

                 */

                // run every two hours on the hour
                // scheduler.Schedule<CVScraperInvocable>().Cron("00 */2 * * *");
                #endregion

            }).OnError((exception) =>
                {
                    WriteLine("It's broken!");
                    WriteLine(exception.Message);
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
                    services.AddTransient<CVScraperInvocable>();
                    services.AddTransient<cvApiTrackerInvocable>(); 

                    services.AddSingleton<IConfiguration>(Program.Configuration);

                    scrapeConfig = new ScrapeConfig();
                    Configuration.GetSection("ScrapeConfiguration").Bind(scrapeConfig);
                    services.AddSingleton(scrapeConfig);

                    scrapeCache = new ScrapeCache();
                    services.AddSingleton(scrapeCache);

                    //services.AddHostedService<Worker>();
                });
    }
}
