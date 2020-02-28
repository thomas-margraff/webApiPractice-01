using EasyNetQ;
using EasyNetQ.Common.Connection;
using EasyNetQ.Common.Messages;
using EmailLib;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Console;

namespace Receive
{
    public class ReceiveMessages
    {
        public void Receive(string queue)
        {
            using (var bus = RabbitHutch.CreateBus(Connection.Local()))
            {
                bus.Receive(queue, x => x
                            .Add<CoronaVirusScrapeMessage>(message => HandleCvMessage(message))
                            .Add<TextMessage>(message => HandleTextMessage(message))
                            .Add<EmailMessage>(message => HandleEmailMessage(message))
                            .Add<DownloadFileMessage>(message => HandleDownloadFileMessage(message)));

                Console.WriteLine("Listening for messages. Hit <return> to quit.");
                ReadLine();
            }
        }

        
        private void HandleCvMessage(CoronaVirusScrapeMessage emailMsg)
        {
            try
            {
                int i = 2;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        private void HandleEmailMessage(EmailMessage emailMsg)
        {
            try
            {
                Gmail.Send(emailMsg);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void HandleDownloadFileMessage(DownloadFileMessage txtMsg)
        {
            WriteLine("Download file message");
        }

        private void HandleTextMessage(TextMessage txtMsg)
        {
            WriteLine(txtMsg.Text);
        }

    }
}

