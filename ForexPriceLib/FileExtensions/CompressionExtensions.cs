using ForexPriceLib.Models;
using Ionic.Zip;
using ForexPriceLib.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ForexPriceLib.FileExtensions
{
    public static class CompressionExtensions
    {
        public static void Read(this ForexPrices fxp, DateTime dt, string symbol = "")
        {
            // fxp = new ForexPrices();
            var zipDir = @"I:\\ForexData\\Forexite\\ARCHIVE_PRICES\\ZIP_ORIGINAL";
            var fname = FileUtils.DateToFxiFileName(dt, "zip");
            var fpath = Path.Combine(zipDir, fname);
            if (string.IsNullOrEmpty(symbol))
            {
                fxp.PriceRecords = fxp.PriceRecords.UnzipPrices(fpath).ToList();
            }
            else
            {
                fxp.PriceRecords = fxp.PriceRecords.UnzipPrices(fpath, symbol).ToList();
            }
            // return fxp;
        }

        public static List<ForexPriceRecord> UnzipPrices(this List<ForexPriceRecord> recs, string zipPath, string symbol = "")
        {
            var bytes = UnZipToMemory(zipPath);
            recs = ParseBytes(bytes, symbol);
            return recs;
        }

        private static byte[] UnZipToMemory(string LocalCatalogZip)
        {
            var result = new Dictionary<string, MemoryStream>();
            var ret = new MemoryStream();
            using (ZipFile zip = ZipFile.Read(LocalCatalogZip))
            {
                foreach (ZipEntry e in zip)
                {
                    e.Extract(ret);
                }
            }

            return ((MemoryStream)ret).ToArray();
        }

        public static List<ForexPriceRecord> ParseBytes(byte[] block, string symbol = "")
        {
            List<ForexPriceRecord> priceRecs = new List<ForexPriceRecord>();
            if (block.Length > 0)
            {
                priceRecs.AddRange((from f in
                                        (from f in System.Text.Encoding.Default.GetString(block, 0, block.Length).Split('\r').ToList()
                                         where !f.StartsWith("<") && f.Length > 6
                                         select f.Replace("\n", "")).ToArray()
                                    let rec = f.Split(',')
                                    select new ForexPriceRecord()
                                    {
                                        Symbol = rec[0],
                                        PriceDateTime = ParseDate(rec[1], rec[2]),
                                        Open = double.Parse(rec[3]),
                                        High = double.Parse(rec[4]),
                                        Low = double.Parse(rec[5]),
                                        Close = double.Parse(rec[6]),
                                    }));

                if (!string.IsNullOrEmpty(symbol))
                {
                    priceRecs = priceRecs.Where(r => r.Symbol == symbol).ToList();
                }
                                    
            }
            return priceRecs.ToList();
        }

        public static DateTimeOffset ParseDate(string date, string time)
        {
            //       01234567
            // date="20090121"
            // time="000100"
            // time="235900"
            // time="hhmm00"
            DateTime dt = new DateTime(
                int.Parse(date.Substring(0, 4)),
                int.Parse(date.Substring(4, 2)),
                int.Parse(date.Substring(6, 2)))
                .Add(new TimeSpan(int.Parse(time.Substring(0, 2)), int.Parse(time.Substring(2, 2)), 0));

            DateTimeOffset dto = new DateTimeOffset(dt, new TimeSpan(-1, 0, 0));
            return dto;
        }

    }
}
