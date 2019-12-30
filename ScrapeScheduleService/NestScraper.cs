using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Coravel.Invocable;
using DAL_SqlServer.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ScrapeScheduleService
{
    public class NestScraper : IInvocable
    {
        public Task Invoke()
        {
            Console.WriteLine("Before scrape");
            var recs = this.InvokeScraper();
            Console.WriteLine("after scrape");

            return Task.CompletedTask;
        }

        public List<IndicatorData> InvokeScraper()
        {
            string url = "http://localhost:7000/api/scrape/getscrape/";
            string jsonData = CallRestMethod(url);
            var recs = JsonConvert.DeserializeObject<List<IndicatorData>>(jsonData);
            jsonData = JsonConvert.SerializeObject(recs, Formatting.Indented);
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
