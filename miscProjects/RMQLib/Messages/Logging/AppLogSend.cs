using System;
using System.Collections.Generic;
using System.Text;

namespace RMQLib.Messages.Logging
{
    public class AppLogSend
    {
        RabbitContext _ctx;
        RmqSender _sender;

        public Guid Id { get; set; }
        public string AppName { get; set; }

        public AppLogSend(RabbitContext ctx)
        {
            this.Id = Guid.NewGuid();
            _ctx = ctx;
            _sender = new RmqSender(_ctx);
        }

    }
}
