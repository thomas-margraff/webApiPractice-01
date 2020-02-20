using System;
using System.Collections.Generic;
using System.Text;

namespace TestLib.Messages
{
    public class RabbitMessage
    {
        public string MessageName { get; set; }
        public object Data { get; set; }
        public string Payload { get; set; }

        public RabbitMessage(string msgName, object data)
        {
            this.MessageName = msgName;
            this.Data = data;
        }
    }
}
