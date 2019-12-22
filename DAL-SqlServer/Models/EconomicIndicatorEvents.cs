using System;
using System.Collections.Generic;

namespace DAL_SqlServer.Models
{
    public partial class EconomicIndicatorEvents
    {
        public int Eieid { get; set; }
        public int IndicatorId { get; set; }
        public DateTime? EventDateTime { get; set; }
        public string EventTime { get; set; }
        public string Actual { get; set; }
        public string Forecast { get; set; }
        public string Revised { get; set; }
        public string Previous { get; set; }
        public string Impact { get; set; }

        public virtual EconomicIndicators Indicator { get; set; }
    }
}
