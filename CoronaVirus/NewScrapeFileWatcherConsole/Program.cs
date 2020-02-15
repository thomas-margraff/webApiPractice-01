using System;
using CoronaVirusDAL;
using CoronaVirusLib;
using Microsoft.Extensions.Configuration;
using CoronaVirusLib.Configuration;
using CoronaVirusLib.Parsers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace NewScrapeFileWatcherConsole
{
    class Program
    {
        static IConfigurationRoot configuration { get; set; }
        private static IServiceProvider _serviceProvider;

        static void Main(string[] args)
        {
            var startup = new ConsoleStartup();

            configuration = startup.CreateConfig();

            var services = startup.createServices();
            _serviceProvider = services.BuildServiceProvider(true);

            IServiceScope scope = _serviceProvider.CreateScope();
            scope.ServiceProvider.GetRequiredService<RabbitMQReceiver>().Run();
            startup.DisposeServices();
        }
    }
}
