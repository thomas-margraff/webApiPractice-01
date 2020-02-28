using System;
using EasyNetQ;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Send
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
            services.AddSingleton<IConfiguration>(configuration);
            services.AddScoped<ConsoleApplication>();

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
