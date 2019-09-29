using System;
using System.Collections.Generic;

namespace DAL_SqlServer
{
    public partial class VwIndicatorCountry
    {
        public int CountryId { get; set; }
        public string CountryCode { get; set; }
        public int IndicatorId { get; set; }
        public string IndicatorName { get; set; }
        public bool IndicatorActive { get; set; }
        public bool CountryActive { get; set; }
    }
}
