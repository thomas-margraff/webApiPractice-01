using CoronaVirusLib.Parsers;
using CoronaVirusLib.ApiTracker;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace coronavirus.tracker.api
{
    public class GetApiData
    {
		public async Task GetData()
		{
			var api = new GetDataFromApi();
			string json = await api.GetJsonData();

			ParseApiTrackerModel parser = new ParseApiTrackerModel();
			var cv = parser.Parse(json);

			var recs = cv.confirmed.locations.Where(r => r.country == "US")
			.Select(r => new
			{
				r.country,
				r.province,
				r.latest
			});

			string dashes = new String('=', 55);
			Clear();
			WriteLine();
			ForegroundColor = ConsoleColor.Green;
			WriteLine("As of {0}", DateTime.Now.ToString("MM.dd.yyyy hh:mm"));
			ForegroundColor = ConsoleColor.White;
			WriteLine(dashes);
			WriteLine($"{"City/State".PadRight(45)} latest");
			WriteLine(dashes);
			foreach (var rec in recs)
			{
				WriteLine($"{rec.province.PadRight(50)} {rec.latest}");
			}
			WriteLine(dashes);
			WriteLine($"{"Total".PadRight(50)} {recs.Sum(r => r.latest)}");
			WriteLine(dashes);
		}
	}
}
