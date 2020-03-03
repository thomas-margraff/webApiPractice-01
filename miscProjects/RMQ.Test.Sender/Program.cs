using CoronaVirusLib.ApiTracker;
using CoronaVirusLib.Messages;
using RMQLib;
using RMQLib.Messages.Email;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RMQ.Test.Sender
{
    class Program
    {
        private static RmqSender _sender;
        private static RabbitContext _ctx;

        static void Main(string[] args)
        {
            apiTrackerSender();
            // emailSender();

            Console.WriteLine("press enter to continue...");
            Console.ReadLine();
        }

        static void apiTrackerSender()
        {
            _ctx = new RabbitContext().Create("cv.scraper.json");
            _sender = new RmqSender(_ctx);

            CoronaVirusApiTrackerMessage msg = CallApi().Result;
            _sender.SendMessage(msg);
        }

        static async Task<CoronaVirusApiTrackerMessage> CallApi()
        {
            var api = new GetDataFromApi();
            string json = await api.GetJsonData();

            var msg = new CoronaVirusApiTrackerMessage();
            msg.PayloadJson = json;

            return msg;
        }

        static void emailSender()
        {
            _ctx = new RabbitContext().Create("cv.localhost.json");
            _sender = new RmqSender(_ctx);

            var message = new EmailMessage()
            {
                Subject = "subject here",
                Body = "new email body",
                IsBodyHtml = false,
                Recipients = new List<string>
                {
                    { "tmargraff@gmail.com" },
                    { "tmargraff.dev@gmail.com" },
                }
            };

            _sender.SendMessage(message);

        }
    }
}
