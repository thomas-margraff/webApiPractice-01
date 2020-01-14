using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;

namespace BulkDataOps
{
    public class PricesDownloader
    {
        public void DownloadPrices()
        {
            // FXIT-20171013
            // var dt = new DateTime(2020, 1, 12);
            var dt = this.getLastScrape();
            while (dt <= DateTime.Today)
            {
                if (dt.DayOfWeek == DayOfWeek.Saturday)
                {
                    dt = dt.AddDays(1);
                    continue;
                }

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
                    client.DownloadFile(new Uri(url), Path.Combine(@"I:\ForexData\Forexite\ARCHIVE_PRICES\ZIP_ORIGINAL", fname));
                }
                dt = dt.AddDays(1);

                Thread.Sleep(2500);
            }
        }

        private DateTime getLastScrape()
        {
            var file = Directory.GetFiles(@"I:\ForexData\Forexite\ARCHIVE_PRICES\ZIP_ORIGINAL").OrderByDescending(r => r).FirstOrDefault();
            var finfo = new FileInfo(file);
            var fname = finfo.Name.Replace("FXIT-", "").Substring(0, 8);
            var yyyy = Convert.ToInt16(fname.Substring(0, 4));
            var mm = Convert.ToInt16(fname.Substring(4, 2));
            var dd = Convert.ToInt16(fname.Substring(6, 2));

            var dt = new DateTime(yyyy, mm, dd);

            return dt;
        }
    }
}
