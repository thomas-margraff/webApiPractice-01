using System;
using System.Collections.Generic;

namespace DAL_SqlServer.Models
{
    public partial class Prices
    {
        public int PriceId { get; set; }
        public int SymbolId { get; set; }
        public DateTime PriceDateTime { get; set; }
        public double O { get; set; }
        public double H { get; set; }
        public double L { get; set; }
        public double C { get; set; }
        public double Bo { get; set; }
        public double Bh { get; set; }
        public double Bl { get; set; }
        public double Bc { get; set; }
        public double Ao { get; set; }
        public double Ah { get; set; }
        public double Al { get; set; }
        public double Ac { get; set; }

        public virtual Symbols Symbol { get; set; }
    }
}
