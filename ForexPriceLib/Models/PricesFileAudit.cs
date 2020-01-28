using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ForexPriceLib.Models
{
    public class PricesFileAudit
    {
        public string Symbol { get; set; }
        public DateTime PriceDate { get; set; }
        public DateTime PriceDateBegin { get; set; }
        public DateTime PriceDateEnd { get; set; }
        public int PriceRecordCount { get; set; }

        public int NumberOfDays
        {
            get
            {
                return (this.PriceDateEnd - this.PriceDateBegin).Days + 1;
            }
        }
        public string PriceDateDayOfWeek
        {
            get
            {
                return this.PriceDate.DayOfWeek.ToString();
            }
        }

        public PricesFileAudit() { }

        public override string ToString()
        {
            return string.Format("{0} {1} {2} {3} {4} {5} {6}",
                this.Symbol,
                this.PriceDate.ToShortDateString(),
                this.PriceDateDayOfWeek,
                this.PriceDateBegin.ToString("MM/dd/yyyy HH:mm"),
                this.PriceDateEnd.ToString("MM/dd/yyyy HH:mm"),
                this.PriceRecordCount,
                this.NumberOfDays);
        }

        public static string CsvHeader()
        {
            return "ccy,Date,DayOfWeek,DateBegin,End,PriceCount,Days";

        }
        public string ToCsv()
        {
            return string.Format("{0},{1},{2},{3},{4},{5},{6}",
                this.Symbol,
                this.PriceDate.ToShortDateString(),
                this.PriceDateDayOfWeek,
                this.PriceDateBegin.ToString("MM/dd/yyyy HH:mm"),
                this.PriceDateEnd.ToString("MM/dd/yyyy HH:mm"),
                this.PriceRecordCount,
                this.NumberOfDays);
        }

    }
}
