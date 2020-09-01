using ForexPriceLib.Models;
using ForexPriceLib.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ForexPriceLib.FileExtensions
{
    public static class FileExtensions
    {
        public static List<ForexPriceRecord> ToCsvFile1(this List<ForexPriceRecord> recs, string filePath)
        {
            File.WriteAllLines(filePath, recs.ToCsvList1().ToArray());
            return recs;
        }

        public static List<string> ToCsvList1(this List<ForexPriceRecord> recs)
        {
            List<string> csvRecs = new List<string>();
            recs.ForEach(r =>
            {
                csvRecs.Add(r.ToCsv1());
            }); 
            return csvRecs;
        }


        /// <summary>
        /// copy to csv file
        /// </summary>
        /// <param name="recs"></param>
        /// <param name="filePath"></param>
        public static void ToCsvFileOHLC(this List<ForexPriceRecord> recs, string filePath, bool withHeader = false)
        {
            File.WriteAllLines(filePath, recs.ToCsvListOHLC(withHeader).ToArray());
        }

        /// <summary>
        ///  copy to csvlist
        /// </summary>
        /// <param name="recs"></param>
        /// <returns></returns>
        public static List<string> ToCsvList(this List<ForexPriceRecord> recs)
        {
            List<string> csvRecs = new List<string>();
            recs.ForEach(r =>
            {
                csvRecs.Add(r.ToCsv());
            });
            return csvRecs;
        }

        public static string ToCsvString(this List<ForexPriceRecord> recs)
        {
            List<string> csvRecs = ToCsvList(recs);
            StringBuilder sb = new StringBuilder();
            csvRecs.ForEach(r =>
            {
                sb.AppendLine(r);
            });
            return sb.ToString();
        }

        public static List<string> ToCsvListOHLC(this List<ForexPriceRecord> recs, bool withHeader = false )
        {
            List<string> csvRecs = new List<string>();
            if (withHeader)
            {
                csvRecs.Add(ForexPriceRecord.CsvHeaderOHLC);
            }
            recs.ForEach(r =>
            {
                csvRecs.Add(r.ToCsvOHLC());
            });
            return csvRecs;
        }

        public static string ToCsvStringOHLC(this List<ForexPriceRecord> recs)
        {
            List<string> csvRecs = ToCsvListOHLC(recs);
            StringBuilder sb = new StringBuilder();
            csvRecs.ForEach(r =>
            {
                sb.AppendLine(r);
            });
            return sb.ToString();
        }

        public static string ToJson(this ForexPrices fxp)
        {
            return JsonConvert.SerializeObject(fxp, Formatting.Indented);
        }

        public static string ToJson(this List<ForexTimePeriodStats> timeStats)
        {
            return JsonConvert.SerializeObject(timeStats, Formatting.Indented);
        }
        public static List<string> ToCsv(this List<ForexTimePeriodStats> timeStats, bool withHeader = false)
        {
            List<string> csv = new List<string>();

            if (withHeader)
                csv.Add(ForexTimePeriodStats.ToTimeStatCsvHeader());

            foreach (ForexTimePeriodStats rec in timeStats)
            {
                csv.Add(rec.ToTimeStatCsv());
            }
            return csv;
        }

        public static string ToJson(this List<ForexPriceRecord> recs)
        {
            return JsonConvert.SerializeObject(recs.ToCsvListOHLC(), Formatting.Indented);
        }
    }
}
