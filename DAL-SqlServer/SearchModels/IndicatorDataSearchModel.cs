using System;
using System.Collections.Generic;
using System.Text;

namespace DAL_SqlServer.SearchModels
{
    public class IndicatorDataSearchModel
    {
        public DateTime ReleaseDateTime { get; set; }
        public DateTime ReleaseDateTimeBegin { get; set; }
        public DateTime ReleaseDateTimeEnd { get; set; }
        public string ReleaseDate { get; set; }
        public string ReleaseTime { get; set; }
        public string Currency { get; set; }
        public string Indicator { get; set; }

    }
}
