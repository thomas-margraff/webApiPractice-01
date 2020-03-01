using Coravel.Invocable;
using EmailLib;
using RMQLib;
using RMQLib.Messages;
using Scrappy;
using System;
using ScrapeServiceWorker.Configuration;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CoronaVirusLib.Messages;

namespace ScrapeServiceWorker.CoronaVirusScraper
{
    public class CVScraperInvocable : IInvocable
    {
        string subject = "Corona Virus Update!!";
        string body = "";
        private bool isRunning = false;
        private readonly ScrapeCache _scrapeCache;
        private readonly ScrapeConfig _scrapeConfig;
        private readonly RabbitContext _ctx;
        private readonly Sender _sender;

        public CVScraperInvocable(ScrapeCache scrapeCache, ScrapeConfig scrapeConfig)
        {
            this._scrapeCache = scrapeCache;
            this._scrapeConfig = scrapeConfig;
            _ctx = new RabbitContext().Create(scrapeConfig.CoronaVirusScrape.GetConfigFile());
            _sender = new Sender(_ctx);
        }

        public Task Invoke()
        {
            if (isRunning)
                return Task.CompletedTask;

            isRunning = true;

            // test
            if (_scrapeConfig.CoronaVirusScrape.IsScheduleDebug)
                _sender.SendMessage(new CoronaVirusScrapeMessage());

            scrapeAndSend().Wait();
            isRunning = false;
            return Task.CompletedTask;
        }

        private async Task scrapeAndSend()
        {
            string html = "";
            var browser = new Browser();
            var url = _scrapeConfig.CoronaVirusScrape.ScrapeUrl;
            var page = await browser.Open(url);
            html = page.Select("#mvp-content-main").Text();

            // The table below shows confirmed cases of coronavirus (2019-nCoV) in China and other countries. To see a distribution map and a timeline, scroll down. 
            // <strong>There are currently 9,171 confirmed cases worldwide, including 213 fatalities.</strong>

            var innerHtml = page.Select("#mvp-content-main").Children()[1].InnerHTML;

            innerHtml = innerHtml.Replace("</strong>", "");
            var parts = innerHtml.Split(">");

            if (this._scrapeCache.PreviousCVscrapeValue != parts[1])
            {
                // send scrape notice
                _sender.SendMessage(new CoronaVirusScrapeMessage());

                // send email notice
                this._scrapeCache.PreviousCVscrapeValue = parts[1];
                body = string.Format("<h2>{0}:<br>{1}</h2>", DateTime.Now, parts[1]);

                Console.WriteLine("{0} {1} CHANGED", DateTime.Now, parts[1]);
                // 
                List<string> recipients = new List<string>{
                    { "thomaspmar@hotmail.com" },
                    { "tmargraff@gmail.com" },
                    { "jmarg538@gmail.com" },
                    { "olsagaming@gmail.com" }
                };

                Gmail.Send(subject, body, recipients, true);
            }
            else
            {
                Console.WriteLine("{0} {1} no change", DateTime.Now, parts[1]);
            }
        }
    }
}
