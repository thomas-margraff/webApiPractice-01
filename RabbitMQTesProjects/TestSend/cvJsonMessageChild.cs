using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TestLib;
using TestLib.Messages;

namespace TestSend
{
    public class cvJsonMessageChild : Sender
    {
        private RabbitContext ctx;

        public cvJsonMessageChild(RabbitContext ctx): base(ctx)
        {
            this.ctx = ctx;
        }

        public void Publish(RabbitMessage msg)
        {
            this.Send(msg);
        }
    }
}
