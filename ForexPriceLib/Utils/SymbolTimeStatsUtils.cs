using ForexPriceLib.FileExtensions;
using ForexPriceLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ForexPriceLib.Utils
{
    public class SymbolTimeStatsUtils
    {
        public List<ForexTimePeriodStats> GetTimeStats(List<DateTime> dts, string symbol = "")
        {
            List<ForexTimePeriodStats> recs = new List<ForexTimePeriodStats>();

            foreach (var dt in dts)
            {
                var fxp = new ForexPrices();
                try
                {
                    fxp.Read(dt, symbol);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    continue;
                }
                fxp.PriceRecords = fxp.PriceRecords
                    .Where(r => r.PriceDateTime.TimeOfDay >= dt.TimeOfDay && r.PriceDateTime.TimeOfDay <= dt.AddMinutes(180).TimeOfDay)
                    .ToList();
                
                if (fxp.SymbolTimeStats.Count() > 0)
                {
                    recs.AddRange(fxp.SymbolTimeStats);
                }
            }

            return recs;
        }
    }
}
