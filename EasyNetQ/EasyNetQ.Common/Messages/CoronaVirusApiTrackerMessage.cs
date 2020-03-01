using System;
using System.Collections.Generic;
using System.Text;

namespace EasyNetQ.Common.Messages
{
    public class CoronaVirusApiTrackerMessage : BaseMessage
    {
        public string PayloadJson { get; set; }
    }
}
