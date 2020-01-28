using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace ForexDomainObject.Core
{
    /// <summary>
    /// extension methods
    /// </summary>
    public static class ForexDomainObjectExtensions
    {
        public static void ToCsvFileFxcm(this List<ForexPriceRecord> recs, string filePath)
        {
            File.WriteAllLines(filePath, recs.ToCsvListFxcm().ToArray());
        }

        public static string ToCsvStringFxcm(this List<ForexPriceRecord> recs)
        {
            List<string> csvRecs = recs.ToCsvListFxcm();
            StringBuilder sb = new StringBuilder();
            csvRecs.ForEach(r =>
            {
                sb.AppendLine(r);
            });
            return sb.ToString();
        }

        public static List<string> ToCsvListFxcm(this List<ForexPriceRecord> recs)
        {
            List<string> csvRecs = new List<string>();
            recs.ForEach(r =>
            {
                csvRecs.Add(r.ToCsvFxcm());
            });
            return csvRecs;
        }

        public static void ToCsvFile1(this List<ForexPriceRecord> recs, string filePath)
        {
            File.WriteAllLines(filePath, recs.ToCsvList1().ToArray());
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
        public static void ToCsvFile(this List<ForexPriceRecord> recs, string filePath)
        {
            File.WriteAllLines(filePath, recs.ToCsvList().ToArray());
        }

        /// <summary>
        ///  copy to csvlist
        /// </summary>
        /// <param name="recs"></param>
        /// <returns></returns>
        public static List<string> ToCsvList(this List<ForexPriceRecord> recs)
        {
            List<string>csvRecs=new List<string>();
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

        public static List<string> ToCsvListOHLC(this List<ForexPriceRecord> recs)
        {
            List<string> csvRecs = new List<string>();
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

        public static DataSet ToDataSet(this ForexPrices prices)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            try
            {
                ds.Tables[0].Columns.Add("dblPrevious", System.Type.GetType("System.Double"));
                ds.Tables[0].Columns.Add("dblActual", System.Type.GetType("System.Double"));
                ds.Tables[0].Columns.Add("dblConsensus", System.Type.GetType("System.Double"));
                ds.Tables[0].Columns.Add("dblDifference", System.Type.GetType("System.Double"));
                ds.Tables[0].Columns.Add("dblRevised", System.Type.GetType("System.Double"));
                ds.Tables[0].Columns.Add("isSelected", System.Type.GetType("System.Boolean"));

                prices.PriceRecords.ForEach(r =>
                    {

                    });
                                            
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }

    }
}
