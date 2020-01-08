using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DAL_SqlServer;
using DAL_SqlServer.Models;
using DAL_SqlServer.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ntpWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScrapeController : ControllerBase
    {
        private readonly ntpContext _ctx;
        private readonly IIndicatorDataRepository _repository;

        public ScrapeController(ntpContext ctx, IIndicatorDataRepository repository)
        {
            this._ctx = ctx;
            this._repository = repository;
        }

        [HttpGet("GetScrape/")]
        public async Task<IEnumerable<IndicatorData>> GetScrape()
        {
            string jsonData = "";
            string url = "http://localhost:3000/api/v1/scraper/week/this";
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

            var recsUpd = _repository.BulkUpdate(recs);

            return recsUpd;
        }
    }
}