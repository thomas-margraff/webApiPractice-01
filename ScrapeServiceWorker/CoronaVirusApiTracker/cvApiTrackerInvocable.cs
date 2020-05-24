using Coravel.Invocable;
using CoronaVirusLib.ApiTracker;
using CoronaVirusLib.Messages;
using ScrapeServiceWorker.Configuration;
using RMQLib;
using System.Threading;
using System.Threading.Tasks;

namespace ScrapeServiceWorker.CoronaVirusApiTracker
{
    public class cvApiTrackerInvocable : IInvocable
    {
        private bool isRunning = false;
        private readonly ScrapeConfig _scrapeConfig;
        private readonly RmqSender _sender;
        private readonly RabbitContext _ctx;

        public cvApiTrackerInvocable(ScrapeConfig scrapeConfig)
        {
            this._scrapeConfig = scrapeConfig;
            _ctx = new RabbitContext().Create(scrapeConfig.CoronaVirusApiTracker.GetConfigFile());
            _sender = new RmqSender(_ctx);
        }

        public Task Invoke()
        {
            // api broken 4/7/2020
            return Task.CompletedTask;

            //if (isRunning)
            //    return Task.CompletedTask;

            //isRunning = true;

            //CoronaVirusApiTrackerMessage msg = CallApi().Result;
            //_sender.SendMessage(msg);
            
            //isRunning = false;
            
            //return Task.CompletedTask;
        }

        private async Task<CoronaVirusApiTrackerMessage> CallApi()
        {
            var api = new GetDataFromApi();
            string json = await api.GetJsonData();

             var msg = new CoronaVirusApiTrackerMessage();
             msg.PayloadJson = json;

            return msg;
        }
    }
}
