using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NTP.Release.DomainObjects;

namespace NTP.DomainObjects
{
    public class PriceStats
    {
        public string Symbol { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        List<EconomicCalendar> EconomicEvents { get; set; }

    }
}
