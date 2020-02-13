using ForexPriceLib.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace NtpDataLib.Models
{
    [Serializable]
    public class IndicatorPrices
    {
        public int Id { get; set; }
        public string Currency { get; set; }
        public string Indicator { get; set; }
        public List<IndicatorDetail> IndicatorDetails { get; set; }

        public IndicatorPrices()
        {
            this.IndicatorDetails = new List<IndicatorDetail>();
        }
    }

    [Serializable]
    public class IndicatorDetail
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public DateTime ReleaseDateTime { get; set; }
        public string ReleaseDate { get; set; }
        public string ReleaseTime { get; set; }
        public string Actual { get; set; }
        public string Forecast { get; set; }
        public string Previous { get; set; }
        // public List<ForexPrices> ForexPrices { get; set; }

        public List<IndicatorDetailPrices> IndicatorDetailPrices { get; set; }

        public IndicatorDetail()
        {
            // this.ForexPrices = new List<ForexPrices>();
            this.IndicatorDetailPrices = new List<IndicatorDetailPrices>();
        }
    }

    [Serializable]
    public class IndicatorDetailPrices
    {
        public string Symbol { get; set; }
        public List<IndicatorDetailPricesDetail> PricesDetail { get; set; }

        public IndicatorDetailPrices()
        {
            this.PricesDetail = new List<IndicatorDetailPricesDetail>();
        }
    }

    [Serializable]
    public class IndicatorDetailPricesDetail
    {
        public DateTime PriceDateTime { get; set; }
        public double Open { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Close { get; set; }

    }


}
