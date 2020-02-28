using System;
using System.Collections.Generic;
using System.Text;

namespace EasyNetQ.Common.Messages
{
    public class TextMessage : BaseMessage
    {
        public string Text { get; set; }
    }
}
