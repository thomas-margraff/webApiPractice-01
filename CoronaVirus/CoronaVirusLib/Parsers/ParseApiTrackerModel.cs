using CoronaVirusLib.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoronaVirusLib.Parsers
{
    public class ParseApiTrackerModel
    {
        public ApiTrackerModel Parse(string json)
        {
			json = json.Replace("long", "lon");

			List<string> historyRecs = new List<string>();
			var cv = JsonConvert.DeserializeObject<ApiTrackerModel>(json);
			cv.confirmed.locations = ParseLocationItems(cv.confirmed.locations);
			cv.deaths.locations = ParseLocationItems(cv.deaths.locations);
			cv.recovered.locations = ParseLocationItems(cv.recovered.locations);

			return cv;
		}

		public List<LocationsItem> ParseLocationItems(List<LocationsItem> recs)
		{
			if (recs == null)
				return recs;

			foreach (var rec in recs)
			{
				string history = rec.history.ToString()
					.Replace("history: {", "")
					.Replace("{", "")
					.Replace("}", "")
					.Replace(" ", "")
					.Trim();

				var hrecs = history.Split(',').ToList();
				foreach (var h in hrecs)
				{
					HistoryRec hh = new HistoryRec();
					var hitem = h.Split(':');
					try
					{
						hh.dt = DateTime.Parse(hitem[0].Replace("\"", "")).Date;
						hh.Count = int.Parse(hitem[1].Replace("\"", ""));
					}
					catch (Exception ex)
					{

					}
					rec.HistoryRecs.Add(hh);
				}
				rec.history = "";
				rec.HistoryRecs = rec.HistoryRecs.OrderBy(hr => hr.dt).ToList();
			}
			return recs;
		}

	}
}
