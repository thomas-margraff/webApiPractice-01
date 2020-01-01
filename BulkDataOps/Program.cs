using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using DAL_SqlServer;
using DAL_SqlServer.Models;
using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace BulkDataOps
{
    class Program
    {
        static async Task Main(string[] args)
        {
            BulkOps ops = new BulkOps();
            // ops.BulkUpdate();

            Tester t = new Tester();
            await t.GetRecs();
            
            Console.WriteLine("DONE");
        }


        public static async void getRecs()
        {
            var url = "http://localhost:3000/api/v1/scraper/week/this";
            url = "http://localhost:7000/api/scrape/getscrape/";
            using (var client = new HttpClient())
            {
                var content = await client.GetStringAsync(url);
                Console.WriteLine(content);
            }
            
        }

    }
}
