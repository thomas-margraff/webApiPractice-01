using RMQLib;
using RMQLib.Messages.Email;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace RMQ.Test.Sender
{
    public class EmailSender
    {
        private RmqSender _sender;

        public EmailSender(RabbitContext ctx)
        {
            _sender = new RMQLib.RmqSender(ctx);
        }

        public void Send(EmailMessage msg)
        {
            _sender.SendMessage(msg);
        }
    }
}
