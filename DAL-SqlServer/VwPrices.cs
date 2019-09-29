using System;
using System.Collections.Generic;

namespace DAL_SqlServer
{
    public partial class VwPrices
    {
        public string SymbolCode { get; set; }
        public DateTime PriceDateTime { get; set; }
        public double O { get; set; }
        public double H { get; set; }
        public double L { get; set; }
        public double C { get; set; }
        public int SymbolId { get; set; }
        public int PriceId { get; set; }
    }
}
