using ForexPriceLib.Utils;
using ForexPriceLib.FileExtensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ForexPriceLib.Models
{
    public class ForexiteFile
    {
        public DateTime PricesDate { get; set; }
        public string FileSystemFolder { get; set; }

        public ForexiteFile() { }
        public ForexiteFile(DateTime dt)
        {
            this.PricesDate = dt;
            this.FileSystemFolder = @"I:\ForexData\Forexite\ARCHIVE_PRICES\ZIP_ORIGINAL";
        }
        public ForexiteFile(DateTime dt, string fileFolder)
        {
            this.PricesDate = dt;
            this.FileSystemFolder = fileFolder;
        }

        public string Name()
        {
            return FileUtils.DateToFileName(this.PricesDate);
        }
        public string NameZipFile()
        {
            return this.Name() + ".zip";
        }

        public string ForexiteUrl()
        {
            var urlDt = FileUtils.DateToUrlFileName(this.PricesDate);
            return "https://www.forexite.com/free_forex_quotes/" + urlDt + ".zip";
        }

        public string LocalFilePath()
        {
            var zipFile = this.NameZipFile();
            return Path.Combine(this.FileSystemFolder, zipFile);
        }

        public ForexPrices GetForexPrices()
        {
            return this.GetForexPrices(DateTime.Now);
        }

        public ForexPrices GetForexPrices(string symbol)
        {
            return this.GetForexPrices(this.PricesDate, symbol);
        }

        public ForexPrices GetForexPrices(DateTime dt, string symbol = "")
        {
            this.PricesDate = dt;
            var recs = this.GetPriceRecords(symbol);
            var fxp = new ForexPrices(recs);
            return fxp;
        }

        public List<ForexPriceRecord> GetPriceRecords(DateTime dt, string symbol = "")
        {
            this.PricesDate = dt;
            return this.GetPriceRecords(symbol);
        }

        public List<ForexPriceRecord> GetPriceRecords(string symbol = "")
        {
            List<ForexPriceRecord> recs = new List<ForexPriceRecord>();

            try
            {
                if (!File.Exists(this.LocalFilePath()))
                {
                    this.DownloadFile();
                }
                if (!string.IsNullOrEmpty(symbol))
                    recs = recs.UnzipPrices(this.LocalFilePath(),"EURUSD");
                else
                    recs = recs.UnzipPrices(this.LocalFilePath());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return recs;
        }

        public void DownloadFile()
        {
            Downloader dl = new Downloader();
            try
            {
                dl.DownloadPriceFile(this.PricesDate, this.FileSystemFolder);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
