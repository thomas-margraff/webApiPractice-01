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
using HtmlAgilityPack;
using static System.Console;
using System.Linq;
using System.IO;

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
        private readonly RmqSender _sender;

        public CVScraperInvocable(ScrapeCache scrapeCache, ScrapeConfig scrapeConfig)
        {
            this._scrapeCache = scrapeCache;
            this._scrapeConfig = scrapeConfig;
            _ctx = new RabbitContext().Create(scrapeConfig.CoronaVirusScrape.GetConfigFile());
            _sender = new RmqSender(_ctx);
        }

        public Task Invoke()
        {
            if (isRunning)
                return Task.CompletedTask;

            isRunning = true;

            // test - html changed cant parse at this time
            //if (_scrapeConfig.CoronaVirusScrape.IsScheduleDebug)
            //    _sender.SendMessage(new CoronaVirusScrapeMessage());
            try
            {
                scrapeAndSend();
            }
            catch (Exception ex)
            {
                WriteLine("scrapeAndSend failed");
                WriteLine(ex.Message);
            }
            isRunning = false;
            return Task.CompletedTask;
        }

        private void scrapeAndSend()
        {
            string html = "";
            var browser = new Browser();
            var url = _scrapeConfig.CoronaVirusScrape.ScrapeUrl;
            var ss = "https://docs.google.com/spreadsheets";

            var web = new HtmlWeb();
            var doc = web.Load(url);

            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//iframe[@src]");
            foreach (var node in nodes)
            {
                HtmlAttribute attr = node.Attributes["src"];
                if (attr.Value.ToLower().StartsWith(ss))
                {
                    url = attr.Value;
                    break;
                }
            }

            var fpath = @"C:\devApps\data\coronavirus\html";

            web = new HtmlWeb();
            doc = web.Load(url);

            html = doc.Text;
            var fname = string.Format("{0}-bno.html", DateTime.Now.ToString("yyyy.MM.dd.hh.mm.tt"));
            File.WriteAllText(Path.Combine(fpath, fname), html);

            //*[@id="0"]/div/table
            var sheet = doc.DocumentNode.SelectSingleNode("//*[@id='sheets-viewport']").InnerHtml;
            var htmlTable = doc.DocumentNode.SelectSingleNode("//*[@id='0']/div/table").InnerHtml;
            var cases = doc.DocumentNode.SelectSingleNode("//*[@id='0']/div/table/tbody/tr[5]/td[1]").InnerHtml;
            var deaths  = doc.DocumentNode.SelectSingleNode("//*[@id='0']/div/table/tbody/tr[5]/td[2]").InnerHtml;
            var recovered = doc.DocumentNode.SelectSingleNode("//*[@id='0']/div/table/tbody/tr[5]/td[3]").InnerHtml;
            var disp = string.Format("Cases: {0}  Deaths: {1}  Recovered: {2}", cases, deaths, recovered);

            if (this._scrapeCache.PreviousCVscrapeValue != disp)
            {
                dumpToConsole(doc, true);

                var rowData = parseAndDump(doc);
                
                // send scrape notice
                _sender.SendMessage(new CoronaVirusScrapeMessage());

                // send email notice
                this._scrapeCache.PreviousCVscrapeValue = disp;
                body = string.Format("<h2>{0}:<br>{1}</h2>", DateTime.Now, disp);

                // WriteLine("{0} {1} CHANGED", DateTime.Now, disp);
                // 
                List<string> recipients = new List<string>{
                    { "thomaspmar@hotmail.com" },
                    { "tmargraff@gmail.com" },
                    { "olsagaming@gmail.com" }
                };

                Gmail.Send(subject, body, recipients, true);
            }
            else
            {
                dumpToConsole(doc, false);
                // WriteLine("{0} {1} no change", DateTime.Now, disp);
            }
        }

        private void dumpToConsole(HtmlDocument doc, bool changed)
        {
            // united states
            var cases = doc.DocumentNode.SelectSingleNode("//*[@id='1902046093']/div/table/tbody/tr[3]/td[1]").InnerHtml;
            var deaths = doc.DocumentNode.SelectSingleNode("//*[@id='1902046093']/div/table/tbody/tr[3]/td[2]").InnerHtml;
            var recovered = doc.DocumentNode.SelectSingleNode("//*[@id='1902046093']/div/table/tbody/tr[3]/td[3]").InnerHtml;
            var unResolved = doc.DocumentNode.SelectSingleNode("//*[@id='1902046093']/div/table/tbody/tr[3]/td[5]").InnerHtml;

            // Clear();
            if (changed)
                WriteLine("{0} CHANGED", DateTime.Now);
            else
                WriteLine("{0} no change", DateTime.Now);

            var disp = string.Format("United States Cases: {0}  Deaths: {1}  Recovered: {2}  Unresolved: {3}",
                cases, deaths, recovered, unResolved);
            WriteLine(disp);

            //international
            var intlCases = doc.DocumentNode.SelectSingleNode("//*[@id='0']/div/table/tbody/tr[5]/td[1]").InnerHtml;
            var intlDeaths = doc.DocumentNode.SelectSingleNode("//*[@id='0']/div/table/tbody/tr[5]/td[2]").InnerHtml;
            var intlRecovered = doc.DocumentNode.SelectSingleNode("//*[@id='0']/div/table/tbody/tr[5]/td[3]").InnerHtml;
            var intlUnresolved = doc.DocumentNode.SelectSingleNode("//*[@id='0']/div/table/tbody/tr[5]/td[4]").InnerHtml;

            disp = string.Format("International Cases: {0}  Deaths: {1}  Recovered: {2}  Unresolved: {3}",
                intlCases, intlDeaths, intlRecovered, intlUnresolved);
            WriteLine(disp);

            WriteLine();
        }

        private List<BnoRowData> parseAndDump(HtmlDocument doc)
        {
            List<BnoRowData> rowData = new List<BnoRowData>();
            HtmlNodeCollection rows = doc.DocumentNode.SelectNodes("//*[@id='0']/div/table//tbody//tr");

            foreach (var row in rows)
            {
                // 1 = other places
                // 2 = cases
                // 3 = deaths
                // 4 = serious
                // 5 = critical
                // 6 = recovered
                // 7 = source
                var place = row.ChildNodes[1].InnerHtml;
                if (place.Contains("Currently being updated") ||
                        place.Contains("Updated throughout the day") ||
                        string.IsNullOrWhiteSpace(place) ||
                        place == "OTHER PLACES" ||
                        row.ChildNodes.Count() < 9)
                    continue;

                if (place.Contains("softmerge-inner")) place = row.ChildNodes[1].InnerText;

                var rd = new BnoRowData()
                {
                    Location = place,
                    Cases = row.ChildNodes[2].InnerHtml,
                    Deaths = row.ChildNodes[3].InnerHtml,
                    Serious = row.ChildNodes[4].InnerHtml.Replace("-", ""),
                    Critical = row.ChildNodes[5].InnerHtml.Replace("-", ""),
                    Recovered = row.ChildNodes[6].InnerHtml
                };
                // 
                rowData.Add(rd);
            }
            return rowData;
        }

        private async Task scrapeAndSendScrappy()
        {
            string html = "";
            var browser = new Browser();
            var url = _scrapeConfig.CoronaVirusScrape.ScrapeUrl;
            var page = await browser.Open(url);
            // html = page.Select("#mvp-content-main").Text();
            html = page.Select("#\\30 R1").Text();
            
            // The table below shows confirmed cases of coronavirus (2019-nCoV) in China and other countries. To see a distribution map and a timeline, scroll down. 
            // <strong>There are currently 9,171 confirmed cases worldwide, including 213 fatalities.</strong>

            var innerHtml = page.Select("#mvp-content-main").Children()[1].InnerHTML;

            innerHtml = innerHtml.Replace("</strong>", "");
            var parts = innerHtml.Split(">");
            
            // temp
            this._scrapeCache.PreviousCVscrapeValue = "";

            if (this._scrapeCache.PreviousCVscrapeValue != parts[1])
            {
                // send scrape notice
                _sender.SendMessage(new CoronaVirusScrapeMessage());

                // send email notice
                this._scrapeCache.PreviousCVscrapeValue = parts[1];
                body = string.Format("<h2>{0}:<br>{1}</h2>", DateTime.Now, parts[1]);

                WriteLine("{0} {1} CHANGED", DateTime.Now, parts[1]);
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
                WriteLine("{0} {1} no change", DateTime.Now, parts[1]);
            }
        }
    }

}
