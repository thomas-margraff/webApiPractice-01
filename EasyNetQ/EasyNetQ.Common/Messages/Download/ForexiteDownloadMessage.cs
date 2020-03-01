using System;
using System.Collections.Generic;
using System.Text;

namespace EasyNetQ.Common.Messages.Download
{
    public class ForexiteDownloadMessage: BaseDownloadContentMessage
    {
        public DateTime DownloadDataForDate { get; set; }
        public string ResultMessage { get; set; }
        public byte[] FileBytes { get; set; }

        public ForexiteDownloadMessage():
            base()
        {     
        }
    }
}
