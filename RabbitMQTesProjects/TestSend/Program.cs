using System;
using System.IO;
using System.Text.Json;
using Newtonsoft.Json;
using static System.Console;
using JsonSerializer = System.Text.Json.JsonSerializer;
using RMQLib;
using RMQLib.Messages;

namespace TestSend
{
    class Program
    {
        static void Main(string[] args)
        {
            scrapeServiceWorkerSend();
            // sendJsonMessages();
            // sendLogMessages();
            // sendJsonMessagesWithLogging();
        }

        static void scrapeServiceWorkerSend()
        {
            var sender = new RMQScrapeServiceWorker();
            while (true)
            {
                WriteLine("enter a message");
                var msg = ReadLine();
                sender.Send(msg);
                WriteLine("");
            }
        }
        static void sendLogMessages()
        {
            LogMessage logMsg = new LogMessage();

            while (true)
            {
                WriteLine(" enter a message");
                var msg = ReadLine();
                logMsg.Publish(msg);
                WriteLine(" sent");
                WriteLine("");
            }
        }

        static void sendJsonMessages()
        {
            var ctx = new RabbitContext().Create("cv.scraper.json");
            cvJsonMessageSend cvJsonMsg = new cvJsonMessageSend();

            while (true)
            {
                WriteLine(" enter a message");
                var msg = ReadLine();
                cvJsonMsg.Publish(msg);
                //RabbitMessage rGenMsg = new RabbitMessage("cv.general", msg);
                //cvJsonMsg.Publish(rGenMsg);
                WriteLine(" sent");
                WriteLine("");
            }
        }

        static void sendJsonMessagesWithLogging()
        {
            cvJsonMessageSend cvJsonMsg = new cvJsonMessageSend();
            LogMessage logMsg = new LogMessage();

            while (true)
            {
                WriteLine(" enter a message");
                var msg = ReadLine();
                RabbitMessage rGenMsg = new RabbitMessage("cv.general", msg);
                cvJsonMsg.Publish(rGenMsg.MessageName);
                logMsg.Publish("sent a message: " + msg);
                WriteLine(" sent");
                WriteLine("");
            }
        }

    }
}
