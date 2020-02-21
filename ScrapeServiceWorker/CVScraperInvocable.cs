using Coravel.Invocable;
using EmailLib;
using Scrappy;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RabbitMQLib;
using RabbitMQLib.Messages;

namespace ScrapeServiceWorker
{
    public class CVScraperInvocable : IInvocable
    {
        string subject = "Corona Virus Update!!";
        string body = "";

        RMQMessage _rmqMsg = new RMQMessage("scrapeFileNotification", "scrapeFileNotification", "newscrape");
        RMQMessage _rmqTestMsg = new RMQMessage("scrapeFileNotification", "scrapeFileNotification", "newscrapeTest");
        ScrapeCache _scrapeCache;
        private readonly ScrapeConfig _scrapeConfig;

        public CVScraperInvocable(ScrapeCache scrapeCache, ScrapeConfig scrapeConfig)
        {
            this._scrapeCache = scrapeCache;
            this._scrapeConfig = scrapeConfig;
        }

        public Task Invoke()
        {
            // test
            // RMQSender.Send(_rmqTestMsg);

            scrapeAndSend().Wait();
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
                RMQSender.Send(_rmqMsg);

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
