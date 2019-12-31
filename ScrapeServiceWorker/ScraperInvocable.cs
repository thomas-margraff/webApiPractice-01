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
            Console.WriteLine("Before scrape: " + DateTime.Now);

            var recs = this.InvokeScraper();

            Console.WriteLine("after scrape: " + DateTime.Now);
            Console.WriteLine("recs scraped: " + recs.Count());
            Console.WriteLine("next scrape : " + DateTime.Now.AddMinutes(30));
            string jsonData = JsonConvert.SerializeObject(recs, Formatting.Indented);
            Console.WriteLine(jsonData);

            Console.WriteLine("");


            return Task.CompletedTask;

        }

        public List<IndicatorData> InvokeScraper()
        {
            string url = "http://localhost:7000/api/scrape/getscrape/";
            string jsonData = CallRestMethod(url);
            var recs = JsonConvert.DeserializeObject<List<IndicatorData>>(jsonData);
            
            try
            {
                recs = _repository.BulkUpdate(recs.ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return recs;

        }

        public string CallRestMethod(string url)
        {
            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(url);
            webrequest.Method = "GET";
            HttpWebResponse webresponse = (HttpWebResponse)webrequest.GetResponse();
            Encoding enc = System.Text.Encoding.GetEncoding("utf-8");
            StreamReader responseStream = new StreamReader(webresponse.GetResponseStream(), enc);
            string result = string.Empty;
            result = responseStream.ReadToEnd();
            webresponse.Close();
            return result;
        }
    }
}
