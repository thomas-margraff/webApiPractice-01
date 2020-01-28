using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ForexDomainObject
{
    public static class ForexTimePeriodStatsExtensions
    {
        public static List<string> ToCsvList(this List<ForexTimePeriodStats> records)
        {
            List<string> recs = new List<string>();
            records.ForEach(r => recs.Add(r.ToCsv()));
            return recs;
        }
        //public static void ToCsvFile(this List<ForexTimePeriodStats> records, string Filepath)
        //{
        //    ToCsvFile(records, Filepath, false);
        //}
        public static void ToCsvFile(this List<ForexTimePeriodStats> records, string Filepath)
        {
            List<string> recs = new List<string>();
            try
            {
                recs = (from r in records select r.ToCsv(true)).ToList();
                File.WriteAllLines(Filepath, recs.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void ToCsvFile(this List<ForexTimePeriodStats> records, string Filepath, bool IncludeHeader)
        {
            try
            {
                List<string> recs = (from r in records select r.ToCsv(true)).ToList();
                if (IncludeHeader)
                    recs.Insert(0, ForexTimePeriodStats.CsvHeader());

                File.WriteAllLines(Filepath, recs.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static ForexTimePeriodStats ToObject1(string rec)
        {
            var data = rec.Split(',');
            if (data.Length <= 1)
                return null;

            // return string.Format("{0},{1},{2},{3},{4},{5},{6}", Symbol, DateTimeBegin, DateTimeEnd, Open, High, Low, Close);
            // // AUDJPY,02/01/2002 2:01:00 PM -01:00,02/01/2002 5:00:00 PM -01:00,68.29,68.32,68.03,68.19,29,3,-26
            return new ForexTimePeriodStats()
            {
                Symbol = data[1],
                DateTimeBegin = DateTimeOffset.Parse(data[2]),
                DateTimeEnd = DateTimeOffset.Parse(data[3]),
                Open = double.Parse(data[4]),
                High = double.Parse(data[5]),
                Low = double.Parse(data[6]),
                Close = double.Parse(data[7])
            };
        }
        public static ForexTimePeriodStats ToObject(string rec)
        {
            var data = rec.Split(',');

            // return string.Format("{0},{1},{2},{3},{4},{5},{6}", Symbol, DateTimeBegin, DateTimeEnd, Open, High, Low, Close);
            // // AUDJPY,02/01/2002 2:01:00 PM -01:00,02/01/2002 5:00:00 PM -01:00,68.29,68.32,68.03,68.19,29,3,-26
            return new ForexTimePeriodStats()
            {
                Symbol = data[0],
                DateTimeBegin = DateTimeOffset.Parse(data[1]),
                DateTimeEnd = DateTimeOffset.Parse(data[2]),
                Open = double.Parse(data[3]),
                High = double.Parse(data[4]),
                Low = double.Parse(data[5]),
                Close = double.Parse(data[6])
            };
        }
    }
}
