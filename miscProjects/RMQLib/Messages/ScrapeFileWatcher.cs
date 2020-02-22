using System;
using System.Collections.Generic;
using System.Text;
using static System.Console;

namespace RMQLib.Messages
{
    public class ScrapeFileWatcherSend
    {
        RabbitContext _ctx;
        Sender _sender;

        public ScrapeFileWatcherSend(RabbitContext ctx)
        {
            _ctx = ctx;
            _sender = new Sender(_ctx);
        }

        public void Send(string msg)
        {
            _ctx.Binder.RoutingKey = "importscrape";
            _sender.Send(msg);
        }
    }
}
