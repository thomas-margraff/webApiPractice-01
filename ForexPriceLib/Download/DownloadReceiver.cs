using RMQLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForexPriceLib.Download
{
    public class DownloadReceiver : RMQReceiver
    {
        public DownloadReceiver(RabbitContext ctx) :
            base(ctx)
        {

        }
    }
}
