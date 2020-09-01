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
using ForexPriceLib.Models;

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
            var dt = FileUtils.GetLastScrape(this._scrapeConfig.ForexiteDownload.ForexiteArchivePath);

            while (dt <= DateTime.Today)
            {
                if (dt.DayOfWeek == DayOfWeek.Saturday)
                {
                    dt = dt.AddDays(1);
                    continue;
                }
                ForexiteFile fx = new ForexiteFile(dt, this._scrapeConfig.ForexiteDownload.ForexiteArchivePath);
                emailBody.AppendLine(fx.NameZipFile());

                try
                {
                    fx.DownloadFile();
                    // dl.DownloadPriceFile(dt, this._scrapeConfig.ForexiteDownload.ForexiteArchivePath);
                }
                catch (Exception ex)
                {
                    emailBody.AppendLine("error downloading: " + ex.Message);
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
    }
}
