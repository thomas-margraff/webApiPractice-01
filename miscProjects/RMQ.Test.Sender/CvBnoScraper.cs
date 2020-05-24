using CoronaVirusLib.ApiTracker;
using CoronaVirusLib.Messages;
using CoronaVirusLib.Models;
using CoronaVirusLib.Parsers;
using HtmlAgilityPack;
using Newtonsoft.Json;
using RMQLib;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Console;

namespace RMQ.Test.Sender
{
    public class CvBnoScraper
    {
        private RabbitContext _ctx;
        private RMQSession _session;
        private CvBnoReceiver _receiver;

        public CvBnoScraper()
        {
            _session = new RMQSession("rmq.config.json");
            _session.Create("local", "cv.messages.exchange", "cv.messages.queue", "cvBnoScrape");

           // _receiver = new CvBnoReceiver(_session.RabbitContext);
        }

        public void Run()
        {
            while (true)
            {
                WriteLine("(bno scraper) press enter to send another message...");
                ReadLine();
                var rowData = scrapePage();

                var msg = new CoronaVirusBnoScrapeMessage();
                msg.ScrapeRowData = rowData;

                // send scrape notice
                _session.Sender.Send(msg);
            }
        }

        private List<BnoRowData> scrapePage()
        {
            var url = "https://bnonews.com/index.php/2020/02/the-latest-coronavirus-cases/";
            var ss = "https://docs.google.com/spreadsheets";
            var fpath = @"C:\devApps\data\coronavirus\htmltest";

            var web = new HtmlWeb();
            var doc = web.Load(url);

            string html = doc.Text;
            var fname = string.Format("{0}-bno.html", DateTime.Now.ToString("yyyy.MM.dd.hh.mm.tt"));
            File.WriteAllText(Path.Combine(fpath, fname), html);

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

            web = new HtmlWeb();
            doc = web.Load(url);

            var sheet = doc.DocumentNode.SelectSingleNode("//*[@id='sheets-viewport']").InnerHtml;
            var htmlTable = doc.DocumentNode.SelectSingleNode("//*[@id='0']/div/table").InnerHtml;

            // united states
            var cases = doc.DocumentNode.SelectSingleNode("//*[@id='1902046093']/div/table/tbody/tr[3]/td[1]").InnerHtml;
            var deaths = doc.DocumentNode.SelectSingleNode("//*[@id='1902046093']/div/table/tbody/tr[3]/td[2]").InnerHtml;
            var recovered = doc.DocumentNode.SelectSingleNode("//*[@id='1902046093']/div/table/tbody/tr[3]/td[3]").InnerHtml;
            var unResolved = doc.DocumentNode.SelectSingleNode("//*[@id='1902046093']/div/table/tbody/tr[3]/td[5]").InnerHtml;
            
            // Clear();
            WriteLine(DateTime.Now);
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
            var rowData = parseTableData(doc);
            return rowData;

        }

        private List<BnoRowData> parseTableData(HtmlDocument doc)
        {
            List<BnoRowData> rowData = new List<BnoRowData>();

            // united states
            HtmlNodeCollection rows = doc.DocumentNode.SelectNodes("//*[@id='1902046093']/div/table//tbody//tr");

            // international
            // HtmlNodeCollection rows = doc.DocumentNode.SelectNodes("//*[@id='0']/div/table//tbody//tr");

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
                        place == "CASES" ||
                        place == "UNITED STATES" ||
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

            rowData.RemoveAt(0);
            return rowData;
        }

    }

    public class CvBnoReceiver : RMQReceiver
    {
        private int runCount = 0;
        public CvBnoReceiver(RabbitContext ctx) : base(ctx)
        {
            Register(ctx.Queue.Name, "cvBnoScrape");
        }


        public override bool Process(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return true;

            var msg = JsonConvert.DeserializeObject<CoronaVirusBnoScrapeMessage>(json);
            // Clear();
            var hdr = string.Format("  {0,25} {1,10} {2,10} {3,10} {4,10} {5,10} ",
                "Location", "Cases", "Deaths", "Serious", "Critical", "Recovered");

            ForegroundColor = ConsoleColor.Red;
            WriteLine("As of {0}", DateTime.Now.ToString("MM.dd.yyyy hh:mm ss"));
            ForegroundColor = ConsoleColor.Green;
            WriteLine(hdr);
            ForegroundColor = ConsoleColor.White;

            foreach (var row in msg.ScrapeRowData)
            {
                var line = string.Format("  {0,25} {1,10} {2,10} {3,10} {4,10} {5,10} ", 
                    row.Location.Trim(), row.Cases, row.Deaths, row.Serious, row.Critical, row.Recovered);

                WriteLine(line);
            }

            return true;
        }
    }

}
