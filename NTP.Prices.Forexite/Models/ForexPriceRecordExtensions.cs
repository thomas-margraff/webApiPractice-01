using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Linq.Expressions;
using LinqKit;

namespace ForexDomainObject
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

        //public static List<string> ToCsvListWithPips(this List<ForexPriceRecord> records)
        //{
        //    return (from r in records
        //            select r.ToCsvWithPips()).ToList();
        //}

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

        public static string ToCsvStringCompressed(this List<ForexPriceRecord> records)
        {
            StringBuilder sb = new StringBuilder();
            records.ForEach(r => sb.Append(r.ToCsvCompressed() + '\r'));
            return sb.ToString();
        }

        #region write to csvfile
        public static void ToCsvFileWithPips(this List<ForexPriceRecord> records, string Filepath)
        {
            ToCsvFileWithPips(records, Filepath, false);
        }
        public static void ToCsvFileWithPips(this List<ForexPriceRecord> records, string Filepath, bool IncludeHeader)
        {
            try
            {
                if (IncludeHeader)
                {
                    List<string> recs = (from r in records
                                         select r.ToCsvWithPips()).ToList();
                    recs.Insert(0, ForexPriceRecord.CsvWithPipsHeader());
                    File.WriteAllLines(Filepath, recs.ToArray());
                }
                else
                {
                    File.WriteAllLines(Filepath,
                        (from r in records
                         select r.ToCsv()).ToArray());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void ToCsvFile(this List<ForexPriceRecord> records, string Filepath)
        {
            ToCsvFile(records, Filepath, false);
        }

        public static void ToCsvFile(this List<ForexPriceRecord> records, string Filepath, bool IncludeHeader)
        {
            try
            {
                if (IncludeHeader)
                {
                    List<string> recs = (from r in records
                                         select r.ToCsv()).ToList();
                    recs.Insert(0, ForexPriceRecord.CsvHeader());
                    File.WriteAllLines(Filepath, recs.ToArray());
                }
                else
                {
                    File.WriteAllLines(Filepath,
                        (from r in records
                         select r.ToCsv()).ToArray());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        public static ForexTimePeriodStats OHLCStats(this List<ForexPriceRecord> records, string symbol)
        {
            return (from r in records where r.Symbol==symbol select r).ToList().OHLCStats() ;
        }

        public static ForexTimePeriodStats OHLCStats(this List<ForexPriceRecord> records)
        {
            string symbol;
            if (records.Count==0)
                return new ForexTimePeriodStats();

            symbol=records[0].Symbol;

            ForexTimePeriodStats stats =
                new ForexTimePeriodStats()
                {
                    SymbolId=records[0].SymbolId,
                    Symbol = symbol,
                    DateTimeBegin = records[0].PriceDateTime,
                    DateTimeEnd = records[records.Count - 1].PriceDateTime,
                    Open = records.Open(),
                    High = records.High(),
                    Low = records.Low(),
                    Close = records.Close()
                };

            return stats;
        }

        public static double Open(this List<ForexPriceRecord> records)
        {
            return (from r in records select r.Open).First();
        }

        public static double High(this List<ForexPriceRecord> records)
        {
            return (from r in records select r.High).Max();
        }

        public static double Low(this List<ForexPriceRecord> records)
        {
            return (from r in records select r.Low).Min();
        }

        public static double Close(this List<ForexPriceRecord> records)
        {
            return (from r in records select r.Close).Last();
        }

    }
}
