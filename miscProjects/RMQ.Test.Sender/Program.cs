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
        private static RMQSession _session;
        private static apiTracker _receiver;

        static void Main(string[] args)
        {
            // BnoScraper();
            apiTrackerSenderNew();

            // apiTrackerSender();
            // emailSender();
            // fanoutSender();
            // directSender();
            // sendScrapeMessage();

            Console.WriteLine("press enter to continue...");
            Console.ReadLine();
        }

        static void sendScrapeMessage()
        {
            _ctx = new RabbitContext().Create("cv.scraper.json");
            _sender = new RmqSender(_ctx);

            int cnt = 0;
            while (true)
            {
                cnt++;
                _sender.SendMessage(new CoronaVirusScrapeMessage());
                Console.WriteLine("press enter to send another message...", cnt.ToString()); ;
                Console.ReadLine();
            }
        }

        static void directSender()
        {
            string exchange = "direct_logs";
            string exchangeType = "direct";
            string routingKey = "info";

            _ctx = new RabbitContext().Create(exchange, exchangeType, routingKey);
            _sender = new RmqSender(_ctx);
            int cnt = 0;
            while (true)
            {
                cnt++;
                _sender.Send("info: Hello World! " + cnt.ToString());
                Console.WriteLine("press enter to send another message...");
                Console.ReadLine();
            }
        }

        static void fanoutSender()
        {
            _ctx = new RabbitContext().Create("logs", "fanout");
            _sender = new RmqSender(_ctx);
            int cnt = 0;
            while (true)
            {
                cnt++;
                _sender.Send("info: Hello World! " + cnt.ToString());
                Console.WriteLine("press enter to send another message...");
                Console.ReadLine();
            }
        }

        static void BnoScraper()
        {
            CvBnoScraper scraper = new CvBnoScraper();
            scraper.Run();
        }
        static void apiTrackerSenderNew()
        {
            apiTracker apiTracker = new apiTracker();
            apiTracker.Run();
        }

        static void apiTrackerSender()
        {
            _ctx = new RabbitContext().Create("cv.localhost.json");
            _sender = new RmqSender(_ctx);
            _session.Sender.SetRoutingKey("cvApiTracker", "cvApiTracker");

            while (true)
            {
                Console.WriteLine("(apiTrackerSender) press enter to send another message...");
                Console.ReadLine();
                CoronaVirusApiTrackerMessage msg = CallApi().Result;
                // _session.Sender.Send(msg);
                _sender.SendMessage(msg);
            }
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
