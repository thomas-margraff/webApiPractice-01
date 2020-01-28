using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ForexDomainObject;
using System.Linq.Expressions;

namespace NTP.Prices.Forexite
{
    public class ForexiteFileReader
    {
        public string FileDataPath
        {
            get { return @"C:\tdev\Data\Forexite\Downloads\All"; }
            set { ;}
        }

        //public List<ForexPriceRecord> Read(DateTime priceDate)
        //{
        //    return Read(Path.Combine(FileDataPath, Forexite.DateToFileName1(priceDate)), PredicateBuilder.True<ForexPriceRecord>());
        //}

        //public List<ForexPriceRecord> Read(string zipFilePath)
        //{
        //    return Read(zipFilePath, PredicateBuilder.True<ForexPriceRecord>());
        //}

        public List<ForexPriceRecord> Read(string zipFilePath)
        {
            // file must exist
            if (!File.Exists(zipFilePath))
                throw new FileNotFoundException();

            List<ForexPriceRecord> priceRecs = new List<ForexPriceRecord>();
            byte[] block = ZippedFile.ZipDataStreamBytes(zipFilePath);
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
                                        PriceDateTime = Forexite.ParseDate(rec[1], rec[2]),
                                        Open = double.Parse(rec[3]),
                                        High = double.Parse(rec[4]),
                                        Low = double.Parse(rec[5]),
                                        Close = double.Parse(rec[6]),
                                    })
                                 .ToList());
            }
            return priceRecs;
        }

        //public static List<ForexPriceRecord> ParseBytes(byte[] block)
        //{
        //    return ParseBytes(block, PredicateBuilder.True<ForexPriceRecord>());
        //}
        public static List<ForexPriceRecord> ParseBytes(byte[] block)
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
                                        PriceDateTime = Forexite.ParseDate(rec[1], rec[2]),
                                        Open = double.Parse(rec[3]),
                                        High = double.Parse(rec[4]),
                                        Low = double.Parse(rec[5]),
                                        Close = double.Parse(rec[6]),
                                    })
                                 .ToList());
            }
            return priceRecs;
        }

        #region ToByteArray - return Byte[]
        public byte[] ToByteArray(DateTime dt)
        {
            return ToByteArray(Path.Combine(this.FileDataPath, Forexite.DateToFileName1(dt)));
        }
        public byte[] ToByteArray(string zipFilePath)
        {
            return ZippedFile.ZipDataStreamBytes(zipFilePath);
        }

        #endregion

    }
}
