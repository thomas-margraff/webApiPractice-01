using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.IO;

using NTP.Forexite.DomainObjects;
using Wincorp.CommonLib.DateUtilities;
using Wincorp.CommonLib.ZipFiles;
using Wincorp.CommonLib.Configuration;
using System.Linq.Expressions;
using LinqKit;

namespace NTP.Prices.Forexite
{
    public class ForexiteDataReader
    {
        WincorpConfig _config = new WincorpConfig();
        public string FileDataPath 
        {
            get { return @"C:\tdev\Data\Forexite\Downloads\All"; }
            set { ;} 
        }

        public ForexiteDataReader()
        {
            //FileDataPath = _config.Local.ForexiteZipFilePath+"\\All";
        }

        #region Read - return List<ForexitePriceRecord>

        #region <List>DateTime
        public List<ForexitePriceRecord> Read(List<DateTime> PriceDates, Expression<Func<ForexitePriceRecord, bool>> pred)
        {
            List<ForexitePriceRecord> recs = new List<ForexitePriceRecord>();
            foreach (var date in PriceDates)
                recs.AddRange(this.Read(date, pred));

            return recs.Distinct().ToList();

        }
        public List<ForexitePriceRecord> Read(List<DateTime> PriceDates, TimeSpan ts)
        {
            List<ForexitePriceRecord> recs = new List<ForexitePriceRecord>();
            foreach (DateTime date in PriceDates)
            {
                DateTimeOffset dto = new DateTimeOffset(date, new TimeSpan(0, 0, 0));
                var pred = ForexitePriceRecord.ForTimeSpan(dto, ts);
                recs.AddRange(this.Read(dto).Where(pred.Compile()));
            }
            return recs;
        }
        public List<ForexitePriceRecord> Read(List<DateTime> PriceDates)
        {
            return Read(PriceDates, PredicateBuilder.True<ForexitePriceRecord>()).Distinct().ToList();
        }
        #endregion

        #region List<DateRange>

        public List<ForexitePriceRecord> Read(List<DateRange> PriceDateRanges)
        {
            return Read(PriceDateRanges, PredicateBuilder.True<ForexitePriceRecord>());
        }
        public List<ForexitePriceRecord> Read(List<DateRange> PriceDateRanges, Expression<Func<ForexitePriceRecord, bool>> pred)
        {
            List<ForexitePriceRecord> recs = new List<ForexitePriceRecord>();
            foreach (var dateRange in PriceDateRanges)
            {
                recs.AddRange(Read(dateRange, pred));
            }
            return recs;
        }
        #endregion

        #region DateRange
        public List<ForexitePriceRecord> Read(DateRange PriceDates, Expression<Func<ForexitePriceRecord, bool>> pred)
        {
            List<ForexitePriceRecord> recs = new List<ForexitePriceRecord>();
            foreach (var date in PriceDates)
                recs.AddRange(this.Read(date, pred));

            return recs.Distinct().ToList();
        }
        public List<ForexitePriceRecord> Read(DateRange PriceDates)
        {
            return Read(PriceDates, PredicateBuilder.True<ForexitePriceRecord>());
        }
        #endregion

        #region DateTime
        public List<ForexitePriceRecord> Read(DateTime PriceDate, Expression<Func<ForexitePriceRecord, bool>> pred)
        {
            return Read(Path.Combine(FileDataPath, Forexite.DateToFileName1(PriceDate)), pred);
        }
        public List<ForexitePriceRecord> Read(DateTimeOffset PriceDate, Expression<Func<ForexitePriceRecord, bool>> pred)
        {
            return Read(Path.Combine(FileDataPath, Forexite.DateToFileName1(PriceDate.Date)), pred);
        }
        public List<ForexitePriceRecord> Read(DateTimeOffset PriceDate)
        {
            return Read(Path.Combine(FileDataPath, Forexite.DateToFileName1(PriceDate.Date)), PredicateBuilder.True<ForexitePriceRecord>());
        }
        public List<ForexitePriceRecord> Read(DateTime PriceDate)
        {
            return Read(Path.Combine(FileDataPath, Forexite.DateToFileName1(PriceDate)), PredicateBuilder.True<ForexitePriceRecord>());

            //List<string> recs;

            //try
            //{
            //    recs = ToCsvList(PriceDate);
            //    return (from r in recs
            //            let rec = r.Split(',')
            //            where r.Length > 0 && !r.StartsWith("<")
            //            select new ForexitePriceRecord()
            //            {
            //                Symbol = rec[0],
            //                PriceDateTime = Forexite.ParseDate(rec[1], rec[2]),
            //                Open = double.Parse(rec[3]),
            //                High = double.Parse(rec[4]),
            //                Low = double.Parse(rec[5]),
            //                Close = double.Parse(rec[6]),
            //            }).ToList();
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }
        #endregion

        #region FilePath
        public List<ForexitePriceRecord> Read(string zipFilePath, Expression<Func<ForexitePriceRecord, bool>> pred)
        {
            List<ForexitePriceRecord> priceRecs = new List<ForexitePriceRecord>();
            byte[] block = ZippedFile.ZipDataStreamBytes(zipFilePath);
            if (block.Length > 0)
            {
                priceRecs.AddRange((from f in
                                        (from f in System.Text.Encoding.Default.GetString(block, 0, block.Length).Split('\r').ToList()
                                         where !f.StartsWith("<") && f.Length > 6
                                         select f.Replace("\n", "")).ToArray()
                                    let rec = f.Split(',')
                                    select new ForexitePriceRecord()
                                    {
                                        Symbol = rec[0],
                                        PriceDateTime = Forexite.ParseDate(rec[1], rec[2]),
                                        Open = double.Parse(rec[3]),
                                        High = double.Parse(rec[4]),
                                        Low = double.Parse(rec[5]),
                                        Close = double.Parse(rec[6]),
                                    })
                                 .ToList().Where(pred.Compile()));
            }
            return priceRecs;
        }

        public List<ForexitePriceRecord> Read(string zipFilePath)
        {
            List<ForexitePriceRecord> priceRecs = new List<ForexitePriceRecord>();
            byte[] block = ZippedFile.ZipDataStreamBytes(zipFilePath);
            if (block.Length > 0)
            {
                priceRecs.AddRange((from f in
                                        (from f in System.Text.Encoding.Default.GetString(block, 0, block.Length).Split('\r').ToList()
                                         where !f.StartsWith("<") && f.Length > 6
                                         select f.Replace("\n", "")).ToArray()
                                    let rec = f.Split(',')
                                    select new ForexitePriceRecord()
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

        public static List<ForexitePriceRecord> ReadRecords(string zipFilePath)
        {
            List<ForexitePriceRecord> priceRecs = new List<ForexitePriceRecord>();
            byte[] block = ZippedFile.ZipDataStreamBytes(zipFilePath);
            if (block.Length > 0)
            {
                priceRecs.AddRange((from f in
                                        (from f in System.Text.Encoding.Default.GetString(block, 0, block.Length).Split('\r').ToList()
                                         where !f.StartsWith("<") && f.Length > 6
                                         select f.Replace("\n", "")).ToArray()
                                    let rec = f.Split(',')
                                    select new ForexitePriceRecord()
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
            return priceRecs.ToList();
        }
        #endregion

        #endregion

        #region ToCsvList - return List<string>
        public List<string> ToCsvList(DateTime dt)
        {
            return ToCsvList(Path.Combine(this.FileDataPath, Forexite.DateToFileName1(dt)));
        }

        public List<string> ToCsvList(string zipFilePath)
        {
            byte[] block = ZippedFile.ZipDataStreamBytes(zipFilePath);
            List<string> recs = System.Text.Encoding.Default.GetString(block, 0, block.Length).Split('\r').ToList();
            return (from r in recs
                    where !r.StartsWith("<") && r.Length > 6
                    select r.Replace("\n", "")).ToList();
        }

        #endregion

        #region ToCsvString - return string
        public string ToCsvString(DateTime dt)
        {
            return ToCsvString(Path.Combine(this.FileDataPath, Forexite.DateToFileName1(dt)));
        }
        public string ToCsvString(string zipFilePath)
        {
            byte[] block = ZippedFile.ZipDataStreamBytes(zipFilePath);
            return System.Text.Encoding.Default.GetString(block, 0, block.Length);
        }

        #endregion

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
