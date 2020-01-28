using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ForexDomainObject.Forexite
{
    public class ForexitePriceRecord : ForexPriceRecord
    {
        public ForexitePriceRecord()
            : base()
        {
            base.HasBidAskPrices = false;
        }
    }
}
