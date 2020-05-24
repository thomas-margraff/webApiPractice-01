using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace ForexPriceLib.Utils
{
    public class Downloader
    {
        public void DownloadMissingPriceFiles()
        {
            var dates = this.GetMissingDownloadFiles();
            foreach (var dt in dates)
            {
                try
                {
                    this.DownloadPriceFile(dt);
                }
                catch (Exception ex)
                {
                    throw;
                }
                
                Thread.Sleep(2000);
            }
        }

        public void DownloadPriceFile(DateTime dt)
        {
            string yyyy = dt.Year.ToString();
            string yy = yyyy.Substring(2, 2);
            string mm = dt.Month.ToString();
            if (mm.Length == 1) mm = "0" + mm;
            string dd = dt.Day.ToString();
            if (dd.Length == 1) dd = "0" + dd;

            var fname = string.Format("FXIT-{0}{1}{2}.zip", yyyy, mm, dd);
            Console.WriteLine(fname);

            // "https://www.forexite.com/free_forex_quotes/2020/01/100120.zip";
            // 2020/01/100120
            var urlDt = string.Format("{0}/{1}/{2}{3}{04}", yyyy, mm, dd, mm, yy);

            string url = "https://www.forexite.com/free_forex_quotes/" + urlDt + ".zip";
            using (WebClient client = new WebClient())
            {
                try
                {
                    client.DownloadFile(new Uri(url), Path.Combine(@"I:\ForexData\Forexite\ARCHIVE_PRICES\ZIP_ORIGINAL", fname));
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

        }

        public void DownloadByDate(DateTime dt)
        {
            var dtFmt = string.Format("{0} {1}", DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString());
            Console.WriteLine("Start Price Download at {0}", dtFmt);

            try
            {
                DownloadPrices(dt);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR {0}", ex.Message);
                return;
            }
            Console.WriteLine("End Price Download at {0}", dtFmt);
            Console.WriteLine("");
        }

        public void DownloadPrices(DateTime dt)
        {
            // FXIT-20171013
            // var dt = new DateTime(2020, 1, 12);
            
            while (dt <= DateTime.Today)
            {
                if (dt.DayOfWeek == DayOfWeek.Saturday)
                {
                    dt = dt.AddDays(1);
                    continue;
                }
                this.DownloadPriceFile(dt);
                
                Thread.Sleep(2000);

                dt = dt.AddDays(1);
            }
        }

        public List<DateTime> GetMissingDownloadFiles()
        {
            var dt = getFirstScrape();
            var dtLastScrape = getLastScrape();
            dtLastScrape = DateTime.Today.AddDays(-1);
            List<DateTime> missingFiles = new List<DateTime>();

            while (dt <= dtLastScrape)
            {
                if (dt.DayOfWeek == DayOfWeek.Saturday)
                {
                    dt = dt.AddDays(1);
                    continue;
                }
                var fname = FileUtils.DateToFxiFileName(dt, "zip");
                var fpath = Path.Combine(@"I:\ForexData\Forexite\ARCHIVE_PRICES\ZIP_ORIGINAL", fname);
                if (!File.Exists(fpath))
                {
                    Console.WriteLine(fname + "  MISSING!");
                    missingFiles.Add(dt);
                }
                else
                {
                    Console.WriteLine(fname);
                }
                dt = dt.AddDays(1);
            }
            return missingFiles.OrderByDescending(r => r).ToList();

        }

        private DateTime getLastScrape()
        {
            var file = Directory.GetFiles(@"I:\ForexData\Forexite\ARCHIVE_PRICES\ZIP_ORIGINAL").OrderByDescending(r => r).FirstOrDefault();
            var finfo = new FileInfo(file);
            var fname = finfo.Name.ToUpper().Replace("FXIT-", ""); //.Substring(0, 8);
            var yyyy = Convert.ToInt16(fname.Substring(0, 4));
            var mm = Convert.ToInt16(fname.Substring(4, 2));
            var dd = Convert.ToInt16(fname.Substring(6, 2));

            var dt = new DateTime(yyyy, mm, dd);

            return dt;
        }

        private DateTime getFirstScrape()
        {
            var file = Directory.GetFiles(@"I:\ForexData\Forexite\ARCHIVE_PRICES\ZIP_ORIGINAL").OrderBy(r => r).FirstOrDefault();
            var finfo = new FileInfo(file);
            var fname = finfo.Name.ToUpper().Replace("FXIT-", "");    //.Substring(0, 8);
            var yyyy = Convert.ToInt16(fname.Substring(0, 4));
            var mm = Convert.ToInt16(fname.Substring(4, 2));
            var dd = Convert.ToInt16(fname.Substring(6, 2));

            var dt = new DateTime(yyyy, mm, dd);

            return dt;
        }

    }
}
