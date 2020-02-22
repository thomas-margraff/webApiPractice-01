using System;
using CoronaVirusDAL;
using CoronaVirusLib;
using CoronaVirusLib.Configuration;
using CoronaVirusLib.Parsers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NewScrapeFileWatcherConsole;

namespace NewScrapeFileWatcherConsole
{
    public class ConsoleStartup
    {
        private IConfigurationRoot configuration { get; set; }
        private IServiceProvider _serviceProvider;

        public IConfigurationRoot CreateConfig()
        {
            configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
            
            return configuration;
        }

        public ServiceCollection createServices()
        {
            var services = new ServiceCollection();
            services.AddDbContext<CvContext>(options => options.UseSqlServer(configuration.GetConnectionString("cvConnectionString")));
            services.AddSingleton<IConfiguration>(configuration);
            services.AddScoped<ImportRunner>(); 
            services.AddScoped<cvConfig>();
            services.AddScoped<cvParsers>();
            services.AddScoped<ImportScrapeData>();

            return services;
        }

        public void DisposeServices()
        {
            if (_serviceProvider == null)
            {
                return;
            }
            if (_serviceProvider is IDisposable)
            {
                ((IDisposable)_serviceProvider).Dispose();
            }
        }

    }
}
