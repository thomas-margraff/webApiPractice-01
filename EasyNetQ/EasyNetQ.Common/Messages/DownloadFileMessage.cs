using System;
using System.Collections.Generic;
using System.Text;

namespace EasyNetQ.Common.Messages
{
    public class DownloadFileMessage: BaseMessage
    {
        public DateTime StartDateTime { get; set; }
        public DateTime FinisDateTime { get; set; }
        public string Url { get; set; }

        public DownloadFileMessage()
        {
            StartDateTime = DateTime.Now;
        }
    }
}
