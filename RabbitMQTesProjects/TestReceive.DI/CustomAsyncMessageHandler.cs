using Microsoft.Extensions.Logging;
using RabbitMQ.Client.Core.DependencyInjection;
using System.Threading.Tasks;
using static System.Console;

namespace TestReceive.DI
{
    public class CustomAsyncMessageHandler : IAsyncMessageHandler
    {
        readonly ILogger<CustomAsyncMessageHandler> _logger;

        public CustomAsyncMessageHandler(ILogger<CustomAsyncMessageHandler> logger)
        {
            _logger = logger;
        }

        public async Task Handle(string message, string routingKey)
        {
            WriteLine(message);
            // _logger.LogInformation($"A weird example of running something async with message {message} that has been received by {routingKey}.");
            await Task.CompletedTask.ConfigureAwait(false);
        }
    }
}
