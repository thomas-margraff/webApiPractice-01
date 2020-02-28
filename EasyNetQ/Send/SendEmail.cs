using EasyNetQ;
using EasyNetQ.Common.Connection;
using EasyNetQ.Common.Messages;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Send
{
    public class SendEmail
    {
        private readonly IConfiguration configuration;

        public SendEmail(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public void Run()
        {
            using (var bus = RabbitHutch.CreateBus(Connection.Local()))
            {
                DownloadFileMessage downloadFileMessage = new DownloadFileMessage();
                bus.Send<DownloadFileMessage>("downloadfile.forexite", downloadFileMessage);
            }

        }
    }
}
