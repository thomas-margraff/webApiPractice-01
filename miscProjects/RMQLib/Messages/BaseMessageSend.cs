using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using static System.Console;

namespace RMQLib.Messages
{
    public class BaseMessageSend
    {
        RabbitContext _ctx;
        RmqSender _sender;

        public BaseMessageSend(RabbitContext ctx)
        {
            _ctx = ctx;
            _sender = new RmqSender(_ctx);
        }

        public virtual void SetMessageRouting()
        {

        }


    }
}
