using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Send
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
