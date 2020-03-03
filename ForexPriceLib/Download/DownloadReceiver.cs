using RMQLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForexPriceLib.Download
{
    public class DownloadReceiver : Receiver
    {
        public DownloadReceiver(RabbitContext ctx) :
            base(ctx)
        {

        }
    }
}
