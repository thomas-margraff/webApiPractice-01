using RMQLib.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoronaVirusLib.Messages
{
    public class CoronaVirusApiTrackerMessage : BaseDataMessage
    {
        public string PayloadJson { get; set; }

        public CoronaVirusApiTrackerMessage()
        {
            base.RoutingQueueName = "cvApiTracker";
            base.RoutingKey = "cvApiTracker";

            //base.RoutingQueueName = "cv.messages";
            //base.RoutingKey = "cv.messages";
        }
    }
}
