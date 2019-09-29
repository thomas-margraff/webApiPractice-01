using System;
using System.Collections.Generic;

namespace DAL_SqlServer
{
    public partial class Countries
    {
        public Countries()
        {
            EconomicIndicators = new HashSet<EconomicIndicators>();
        }

        public int CountryId { get; set; }
        public string CountryCode { get; set; }
        public bool Active { get; set; }

        public virtual ICollection<EconomicIndicators> EconomicIndicators { get; set; }
    }
}
