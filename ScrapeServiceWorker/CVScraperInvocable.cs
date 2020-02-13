using Coravel.Invocable;
using EmailLib;
using Scrappy;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ScrapeServiceWorker
{
    public class CVScraperInvocable : IInvocable
    {
        string subject = "Corona Virus Update!!";
        string body = "";

        ScrapeCache _scrapeCache;

        public CVScraperInvocable(ScrapeCache scrapeCache)
        {
            this._scrapeCache = scrapeCache;
        }

        public Task Invoke()
        {
            scrapeAndSend().Wait();
            return Task.CompletedTask;
        }

        private async Task scrapeAndSend()
        {
            string html = "";
            var browser = new Browser();
            var page = await browser.Open("https://bnonews.com/index.php/2020/02/the-latest-coronavirus-cases/");
            html = page.Select("#mvp-content-main").Text();

            // The table below shows confirmed cases of coronavirus (2019-nCoV) in China and other countries. To see a distribution map and a timeline, scroll down. 
            // <strong>There are currently 9,171 confirmed cases worldwide, including 213 fatalities.</strong>

            var innerHtml = page.Select("#mvp-content-main").Children()[1].InnerHTML;

            innerHtml = innerHtml.Replace("</strong>", "");
            var parts = innerHtml.Split(">");
            
            if (this._scrapeCache.PreviousCVscrapeValue != parts[1])
            {
                this._scrapeCache.PreviousCVscrapeValue = parts[1];
                body = string.Format("<h2>{0}:<br>{1}</h2>", DateTime.Now, parts[1]);

                Console.WriteLine("{0} {1} email", DateTime.Now, parts[1]);
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
