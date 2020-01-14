using Coravel.Invocable;
using Coravel.Queuing.Interfaces;
using DAL_SqlServer;
using DAL_SqlServer.Models;
using DAL_SqlServer.Repository;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ScrapeServiceWorker
{
    public class PriceDownloaderInvocable: IInvocable
    {
        private readonly ntpContext _ctx;
        private readonly IIndicatorDataRepository _repository;
        private readonly IConfiguration _configuration;
        private readonly ScrapeConfig _scrapeConfig;

        public PriceDownloaderInvocable(ntpContext ctx,
                                IIndicatorDataRepository repository,
                                IConfiguration configuration,
                                ScrapeConfig scrapeConfig)
        {
            this._ctx = ctx;
            this._repository = repository;
            this._configuration = configuration;
            this._scrapeConfig = scrapeConfig;
        }

        public Task Invoke()
        {
            var dtFmt = string.Format("{0} {1}", DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString());
            Console.WriteLine("Start Price Download at {0}", dtFmt);

            try
            {
                this.DownloadPrices();
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR {0}", ex.Message);
                return Task.FromException(ex);
            }
            Console.WriteLine("End Price Download at {0}", dtFmt);
            Console.WriteLine("");
            return Task.CompletedTask;
        }

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
                    try
                    {
                        client.DownloadFile(new Uri(url), Path.Combine(@"I:\ForexData\Forexite\ARCHIVE_PRICES\ZIP_ORIGINAL", fname));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("error downloading: " + ex.Message);
                    }
                    
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
