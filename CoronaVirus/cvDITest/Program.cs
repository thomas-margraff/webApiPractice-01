using System;
using CoronaVirusDAL;
using CoronaVirusLib;
using CoronaVirusLib.Configuration;
using CoronaVirusLib.Parsers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace cvDITest
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
            scope.ServiceProvider.GetRequiredService<ConsoleApplication>().Run();
            startup.DisposeServices();
        }
    }
}
