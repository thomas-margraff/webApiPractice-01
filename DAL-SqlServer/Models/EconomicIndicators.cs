using System;
using System.Collections.Generic;

namespace DAL_SqlServer.Models
{
    public partial class EconomicIndicators
    {
        public EconomicIndicators()
        {
            EconomicIndicatorEvents = new HashSet<EconomicIndicatorEvents>();
        }

        public int IndicatorId { get; set; }
        public int CountryId { get; set; }
        public string IndicatorName { get; set; }
        public bool Active { get; set; }

        public virtual Countries Country { get; set; }
        public virtual ICollection<EconomicIndicatorEvents> EconomicIndicatorEvents { get; set; }
    }
}
