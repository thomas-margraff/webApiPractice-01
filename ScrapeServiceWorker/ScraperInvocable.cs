using Coravel.Invocable;
using Coravel.Queuing.Interfaces;
using DAL_SqlServer;
using DAL_SqlServer.Models;
using DAL_SqlServer.Repository;
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
        
        public ScraperInvocable(ntpContext ctx, IIndicatorDataRepository repository)
        {
            this._ctx = ctx;
            this._repository = repository;
        }

        public Task Invoke()
        {
            this.GetRecs().Wait();
            return Task.CompletedTask;
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
                Console.WriteLine("next scrape : " + DateTime.Now.AddMinutes(60));
                Console.WriteLine("");
            }
        }
    }
}
