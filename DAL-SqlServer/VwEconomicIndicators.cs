using System;
using System.Collections.Generic;

namespace DAL_SqlServer
{
    public partial class VwEconomicIndicators
    {
        public string CountryCode { get; set; }
        public string IndicatorName { get; set; }
        public DateTime? EventDateTime { get; set; }
        public string Actual { get; set; }
        public string Forecast { get; set; }
        public string Revised { get; set; }
        public string Previous { get; set; }
        public string Impact { get; set; }
        public int Eieid { get; set; }
        public int IndicatorId { get; set; }
        public int CountryId { get; set; }
    }
}
