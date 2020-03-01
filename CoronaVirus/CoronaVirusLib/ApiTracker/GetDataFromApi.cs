using CoronaVirusLib.Parsers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace CoronaVirusLib.ApiTracker
{
    public class GetDataFromApi
    {
		public async Task<string> GetJsonData()
		{
			string json = "";
			string url = "https://coronavirus-tracker-api.herokuapp.com/all";
			using (var client = new HttpClient())
			{
				try
				{
					json = await client.GetStringAsync(url);
					json = json.Replace("long", "lon");
				}
				catch (Exception ex)
				{
					throw ex;
				}
			}
			return json;
		}
	}
}
