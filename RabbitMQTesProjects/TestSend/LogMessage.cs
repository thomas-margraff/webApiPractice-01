using System;
using System.Collections.Generic;
using System.Text;
using TestLib;
using TestLib.Messages;

namespace TestSend
{
    public class LogMessage
    {
        private RabbitContext ctx;

        public LogMessage()
        {
            ctx = new RabbitContext().Create("logging.json");
        }

        public void Publish(string msg)
        {
            var sender = new Sender(ctx);
            sender.Send(msg);
        }

        public void Publish(RabbitMessage msg)
        {
            var sender = new Sender(ctx);
            sender.Send(msg);
        }

    }
}
