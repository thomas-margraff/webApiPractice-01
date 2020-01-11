﻿using Coravel.Invocable;
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

namespace ScrapeServiceWorker
{
    public class ScraperInvocable : IInvocable
    {
        private readonly ntpContext _ctx;
        private readonly IIndicatorDataRepository _repository;
        private readonly IConfiguration _configuration;
        private readonly ScrapeConfig _scrapeConfig;

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
            try
            {
                this.DoScrape().Wait();
            }
            catch (Exception ex)
            {
                return Task.FromException(ex);
            }
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<IndicatorData>> DoScrape()
        {
            string jsonData = "";
            string url = this._scrapeConfig.ScrapeUrl;  // "http://localhost:3000/api/v1/scraper/week/this";
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
                var recsUpd = _repository.BulkUpdate(recs);
                return recsUpd;
            }

            return recs;
        }


        public async Task GetRecs()
        {
            Console.WriteLine("Before scrape: " + DateTime.Now);
            var url = "http://localhost:7000/api/scrape/getscrape/";
            using (var client = new HttpClient())
            {
                var json = await client.GetStringAsync(url);
                var recs = JsonConvert.DeserializeObject<List<IndicatorData>>(json);
                Console.WriteLine("after scrape: " + DateTime.Now);
                Console.WriteLine("recs scraped: " + recs.Count());
                Console.WriteLine("");
            }
        }
    }
}