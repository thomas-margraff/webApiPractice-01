using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DAL_SqlServer;
using DAL_SqlServer.Models;
using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http;

using System.Text;
using Microsoft.EntityFrameworkCore;

namespace BulkDataOps
{
    public class Tester
    {
		public async Task GetRecs()
		{
			var url = "http://localhost:7000/api/scrape/getscrape/";
			using (var client = new HttpClient())
			{
				var json = await client.GetStringAsync(url);
				var recs = JsonConvert.DeserializeObject<List<IndicatorData>>(json);
				Console.WriteLine(json);
				Console.WriteLine(recs.Count());
			}
		}

		public async Task<List<IndicatorData>> getRecs1()
		{
			List<IndicatorData> recs = new List<IndicatorData>();
			string url = "http://localhost:3000/api/v1/scraper/week/this";
			using (var client = new HttpClient())
			{
				string jsonData = "";
				try
				{
					jsonData = await client.GetStringAsync(url);
				}
				catch (Exception ex)
				{
					throw;
				}
				
				recs = JsonConvert.DeserializeObject<List<IndicatorData>>(jsonData);
				jsonData = JsonConvert.SerializeObject(recs, Formatting.Indented);
				Console.WriteLine(jsonData);
			}
			return recs;
		}
	}
}
