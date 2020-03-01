using Coravel.Invocable;
using DAL_SqlServer.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using ForexPriceLib.Utils;
using EmailLib;
using ScrapeServiceWorker.Configuration;

namespace ScrapeServiceWorker.Forexite
{
    public class PriceDownloaderInvocable: IInvocable
    {
        private readonly IIndicatorDataRepository _repository;
        private readonly ScrapeConfig _scrapeConfig;
        private bool isRunning = false;
        StringBuilder emailBody;
        string subject = "Forex Prices Download";
        string recipient = "tmargraff@gmail.com";

        public PriceDownloaderInvocable(IIndicatorDataRepository repository, ScrapeConfig scrapeConfig)
        {
            this._repository = repository;
            this._scrapeConfig = scrapeConfig;
        }

        public Task Invoke()
        {
            if (isRunning)
            {
                return Task.CompletedTask;
            }

            emailBody = new StringBuilder();
            isRunning = true;

            var dtFmt = string.Format("{0} {1}", DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString());
            emailBody.AppendLine(string.Format("Start Price Download at {0}", dtFmt));

            try
            {
                this.DownloadPrices();
                this.updateSymbolsList();
            }
            catch (Exception ex)
            {
                isRunning = false;
                emailBody.AppendLine(string.Format("ERROR {0}", ex.Message));
                Console.WriteLine("ERROR " + emailBody.ToString());
                Gmail.Send(subject + " ERROR " + DateTime.Now, emailBody.ToString(), recipient);
                return Task.FromException(ex);
            }
            emailBody.AppendLine(string.Format("End Price Download at {0}", dtFmt));
            emailBody.AppendLine("");
            
            isRunning = false;
            Gmail.Send(subject + " " + DateTime.Now, emailBody.ToString(), recipient);
            Console.WriteLine(emailBody.ToString());
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
                emailBody.AppendLine(fname);

                // "https://www.forexite.com/free_forex_quotes/2020/01/100120.zip";
                // 2020/01/100120
                var urlDt = string.Format("{0}/{1}/{2}{3}{04}", yyyy, mm, dd, mm, yy);

                string url = this._scrapeConfig.ForexiteDownload.ForexiteUrl + urlDt + ".zip";
                using (WebClient client = new WebClient())
                {
                    try
                    {
                        // "I:\ForexData\Forexite\ARCHIVE_PRICES\ZIP_ORIGINAL"
                        client.DownloadFile(new Uri(url), 
                                            Path.Combine(this._scrapeConfig.ForexiteDownload.ForexiteArchivePath, fname));
                    }
                    catch (Exception ex)
                    {
                        emailBody.AppendLine("error downloading: " + ex.Message);
                    }
                }

                dt = dt.AddDays(1);
                Thread.Sleep(2500);
            }
        }
        private void updateSymbolsList()
        {
            // update symbol list
            var sutils = new SymbolUtils();
            var symbols = sutils.GetSymbolListFromFiles();

            var syms = this._repository.BulkUpdateSymbols(symbols);

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
