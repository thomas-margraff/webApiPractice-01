using RMQLib.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace FotexitPriceLib.Download
{
    public class ForexiteDownloadMessage: BaseDataMessage
    {
        public DateTime DownloadDataForDate { get; set; }

        public ForexiteDownloadMessage():
            base()
        {     
        }
    }
}
