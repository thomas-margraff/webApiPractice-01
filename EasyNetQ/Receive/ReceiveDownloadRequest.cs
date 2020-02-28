using EasyNetQ;
using EasyNetQ.Common.Connection;
using EasyNetQ.Common.Messages;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Console;

namespace Receive
{
    public class ReceiveDownloadRequest
    {
        public void Run<T>(string queue, T msg)
        {
            using (var bus = RabbitHutch.CreateBus(Connection.Local()))
            {
                bus.Receive<DownloadFileMessage>("downloadfile.forexite", message => HandleDownloadFileMessage(message));

                Console.WriteLine("Listening for messages. Hit <return> to quit.");
                ReadLine();

            }
        }

        static void HandleDownloadFileMessage(DownloadFileMessage message)
        {
            int i = 0;
        }
    }
}
