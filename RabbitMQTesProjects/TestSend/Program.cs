using System;
using System.IO;
using TestLib;
using TestLib.Messages;
using TestLib.Models;
using TestMessages;
using System.Text.Json;
using Newtonsoft.Json;
using static System.Console;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace TestSend
{
    class Program
    {
        static void Main(string[] args)
        {
            // sendJsonMessages();
            // sendLogMessages();
            sendJsonMessagesWithLogging();
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
            cvJsonMessage cvJsonMsg = new cvJsonMessage();

            while (true)
            {
                WriteLine(" enter a message");
                var msg = ReadLine();
                RabbitMessage rGenMsg = new RabbitMessage("cv.general", msg);
                cvJsonMsg.Publish(rGenMsg);
                WriteLine(" sent");
                WriteLine("");
            }
        }

        static void sendJsonMessagesWithLogging()
        {
            cvJsonMessage cvJsonMsg = new cvJsonMessage();
            LogMessage logMsg = new LogMessage();

            while (true)
            {
                WriteLine(" enter a message");
                var msg = ReadLine();
                RabbitMessage rGenMsg = new RabbitMessage("cv.general", msg);
                cvJsonMsg.Publish(rGenMsg);
                logMsg.Publish("sent a message: " + msg);
                WriteLine(" sent");
                WriteLine("");
            }
        }

    }
}
