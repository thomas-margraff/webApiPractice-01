using System;

namespace EasyNetQ.Common.Messages
{
    public class BaseDownloadContentMessage: BaseMessage
    {
        public string Url { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime FinisDateTime { get; set; }

        public BaseDownloadContentMessage():
            base()
        {

        }

    }

}
