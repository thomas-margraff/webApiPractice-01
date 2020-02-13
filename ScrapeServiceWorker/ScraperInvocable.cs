using Coravel.Invocable;
using Coravel.Queuing.Interfaces;
using DAL_SqlServer;
using DAL_SqlServer.Models;
using DAL_SqlServer.Repository;
using EmailLib;
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

namespace ScrapeServiceWorker
{
    public class ScraperInvocable : IInvocable
    {
        private readonly ntpContext _ctx;
        private readonly IIndicatorDataRepository _repository;
        private readonly IConfiguration _configuration;
        private readonly ScrapeConfig _scrapeConfig;
        StringBuilder emailBody;
        string subject = "Calendar Scrape";
        string recipient = "tmargraff@gmail.com";

        public ScraperInvocable(ntpContext ctx, 
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
            emailBody = new StringBuilder();
            var dtFmt = string.Format("{0} {1}", DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString());
            emailBody.AppendLine(string.Format("Start calendar scrape at {0}", dtFmt));

            try
            {
                this.DoScrape().Wait();
            }
            catch (Exception ex)
            {
                emailBody.AppendLine(string.Format("ERROR {0}", ex.Message));
                Console.WriteLine("ERROR " + emailBody.ToString());
                Gmail.Send(subject + " ERROR " + DateTime.Now, emailBody.ToString(), recipient);

                return Task.FromException(ex);
            }
            
            dtFmt = string.Format("{0} {1}", DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString());
            emailBody.AppendLine(string.Format("End calendar scrape at {0}", dtFmt));
            Gmail.Send(subject + " " + DateTime.Now, emailBody.ToString(), recipient);
            Console.WriteLine(emailBody.ToString());

            return Task.CompletedTask;
        }

        public async Task<IEnumerable<IndicatorData>> DoScrape()
        {
            string jsonData = "";
            string url = this._scrapeConfig.ScrapeUrl;
            using (var client = new HttpClient())
            {
                try
                {
                    jsonData = await client.GetStringAsync(url);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            JObject calendar = JObject.Parse(jsonData);
            List<JToken> results = calendar["calendarData"].Children().ToList();
            List<IndicatorData> recs = new List<IndicatorData>();
            foreach (JToken result in results)
            {
                // JToken.ToObject is a helper method that uses JsonSerializer internally
                IndicatorData rec = result.ToObject<IndicatorData>();
                recs.Add(rec);
            }

            if (this._scrapeConfig.BulkUpdate)
            {
                emailBody.AppendLine(string.Format("Begin update database {0} calendar records", recs.Count()));
                var recsUpd = _repository.BulkUpdate(recs);
                emailBody.AppendLine(string.Format("End   update database {0} calendar records", recs.Count()));
                return recsUpd;
            }

            emailBody.AppendLine(string.Format("Scraped {0} calendar records no database update", recs.Count()));
            return recs;
        }

        //public async Task GetRecs()
        //{
        //    emailBody.AppendLine("Before scrape: " + DateTime.Now);
        //    var url = "http://localhost:7000/api/scrape/getscrape/";
        //    using (var client = new HttpClient())
        //    {
        //        var json = await client.GetStringAsync(url);
        //        var recs = JsonConvert.DeserializeObject<List<IndicatorData>>(json);
        //        emailBody.AppendLine("After scrape: " + DateTime.Now);
        //        emailBody.AppendLine("recs scraped: " + recs.Count());
        //        emailBody.AppendLine("");
        //    }
        //}
    }
}
