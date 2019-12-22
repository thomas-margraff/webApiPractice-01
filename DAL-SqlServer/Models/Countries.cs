using System;
using System.Collections.Generic;

namespace DAL_SqlServer.Models
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


    public static class CountriesEx
    {
        public static List<Countries> ClearForJson(this List<Countries> countries)
        {
            foreach (var country in countries)
            {
                country.ClearForJson();
            }
            return countries;
        }

        public static Countries ClearForJson(this Countries ctry)
        {
            foreach (var item in ctry.EconomicIndicators)
            {
                item.Country = null;
            }
            return ctry;
        }
    }
}
