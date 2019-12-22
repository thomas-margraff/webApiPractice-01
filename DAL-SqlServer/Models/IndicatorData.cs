using System;
using System.Collections.Generic;
using System.Text;

namespace DAL_SqlServer.Models
{
    public class IndicatorData
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public DateTime ReleaseDateTime { get; set; }
        public string ReleaseDate { get; set; }
        public string ReleaseTime { get; set; }
        public string Currency { get; set; }
        public string Indicator { get; set; }
        public string Actual { get; set; }
        public string Forecast { get; set; }
        public string Previous { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? ModifyDate { get; set; }
    }
}
