using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client.Core.DependencyInjection;
using RabbitMQ.Client.Core.DependencyInjection.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TestSend.DI
{
    class Program
    {
        public static async Task Main()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            var serviceProvider = serviceCollection.BuildServiceProvider();
            var queueService = serviceProvider.GetRequiredService<IQueueService>();

            for (var i = 0; i < 10; i++)
            {
                var message = new Message
                {
                    Name = "Custom message",
                    Flag = true,
                    Index = i,
                    Numbers = new[] { 1, 2, 3 }
                };
                await queueService.SendAsync(message, "message", "routing.key");
            }
        }

        static void ConfigureServices(IServiceCollection services)
        {
            var rabbitMqConfiguration = new RabbitMqClientOptions
            {
                HostName = "gull-01.rmq.cloudamqp.com",
                Port = 5672,
                UserName = "noekmbda",
                Password = "jtKtg3LU2uEQOCJRpEgMhdSqf2N7S_Cv",
                VirtualHost = "noekmbda"
            };
            var exchangeOptions = new RabbitMqExchangeOptions
            {
                Type = "topic",
                Durable = false,
                Queues = new List<RabbitMqQueueOptions>
                {
                    new RabbitMqQueueOptions
                    {
                        Name = "testqueue",
                        Durable = false,
                        RoutingKeys = new HashSet<string> { "routing.key" }
                    }
                }
            };
            services.AddRabbitMqClient(rabbitMqConfiguration)
                .AddProductionExchange("message", exchangeOptions);
        }
    }
}
