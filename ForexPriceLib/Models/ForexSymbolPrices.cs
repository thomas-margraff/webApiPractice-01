using ForexPriceLib.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForexPriceLib.Models
{
    public class ForexSymbolPrices
    {
        public string Symbol { get; set; }
        public List<ForexPriceRecord> Prices { get; set; }

        public ForexSymbolPrices()
        {
            this.Prices = new List<ForexPriceRecord>();
        }
    }
}
