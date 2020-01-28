using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ForexDomainObject
{
    public class ForexSymbolPrices
    {
        public string Symbol
        {
            get
            {
                if (PriceRecords == null || PriceRecords.Count == 0)
                    throw new ArgumentNullException("no price records.");

                return PriceRecords[0].Symbol;
            }
        }

        public List<ForexPriceRecord> PriceRecords { get; set; }

        public DateTimeOffset PriceDateFirst 
        {
            get 
            {
                if (PriceRecords == null || PriceRecords.Count == 0)
                    throw new ArgumentNullException("no price records.");

                return PriceRecords[0].PriceDateTime; 
            }
        }
        public DateTimeOffset PriceDateLast
        {
            get
            {
                if (PriceRecords == null || PriceRecords.Count == 0)
                    throw new ArgumentNullException("no price records.");

                return PriceRecords[PriceRecords.Count-1].PriceDateTime;
            }
        }

        public ForexSymbolPrices() { }

    }
}
