using System;
using System.Collections.Generic;
using System.Text;

namespace DAL_SqlServer.Models
{
    public class SymbolPriceTimeStat
    {
        public int Id { get; set; }
        public int IndicatorId { get; set; }
        public string Symbol { get; set; }

    }
}
