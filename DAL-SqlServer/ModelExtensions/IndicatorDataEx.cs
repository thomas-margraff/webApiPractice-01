using DAL_SqlServer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL_SqlServer.ModelExtensions
{
    public static class IndicatorDataEx
    {
        public static string ToCsv(this IndicatorData rec)
        {
            return string.Format("{0},{1},{2},{3},{4}",
                rec.ReleaseDateTime,
                rec.Currency,
                rec.Indicator,
                rec.ReleaseDate,
                rec.ReleaseTime);
        }

        public static List<string> ToCsvList(this List<IndicatorData> recs)
        {
            var csv = new List<string>();
            csv.Add("rdt,ccy,indicator,rd,rt");
            foreach (var rec in recs)
            {
                csv.Add(rec.ToCsv());
            }
            return csv;
        }

        public static string ToCsvString(this List<IndicatorData> recs)
        {
            string csv = "rdt,ccy,indicator,rd,rt" + Environment.NewLine;
            foreach (var rec in recs)
            {
                csv += rec.ToCsv() + Environment.NewLine;
            }
            return csv;
        }

    }
}
