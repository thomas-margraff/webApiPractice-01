using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ForexPriceLib.Models;

namespace ForexPriceLib.Utils
{
    
    public static class ForexPriceRecordExtensions
    {
        public static Dictionary<string, List<ForexPriceRecord>> ToSymbols(this List<ForexPriceRecord> records)
        {
            Dictionary<string, List<ForexPriceRecord>> recs = new Dictionary<string, List<ForexPriceRecord>>();
            List<string> symbols = (from r in records
                                    select r.Symbol).Distinct().ToList();

            foreach (string symbol in symbols)
                recs.Add(symbol, records.ToSymbol(symbol));

            return recs;
        }

        public static List<ForexPriceRecord> ToSymbol(this List<ForexPriceRecord> records, string symbol)
        {
            return (from r in records where r.Symbol == symbol select r).ToList();
        }

        public static List<string> ToCsvList(this List<ForexPriceRecord> records)
        {
            return (from r in records
                    select r.ToCsv()).ToList();
        }
        public static string ToCsvString(this List<ForexPriceRecord> records)
        {
            StringBuilder sb = new StringBuilder();
            records.ForEach(r => sb.Append(r.ToCsv()+'\r'));
            return sb.ToString();
        }

    }
}
