using System;
using System.Collections.Generic;
using System.Text;

namespace coronavirus.tracker.api.Models
{
	public class Root
	{
		public ChildObject confirmed { get; set; }
		public ChildObject deaths { get; set; }
		public LatestValues latest { get; set; }
		public ChildObject recovered { get; set; }
	}

	public class LatestValues
	{
		public int confirmed { get; set; }
		public int deaths { get; set; }
		public int recovered { get; set; }
	}

	public class ChildObject
	{
		public int latest { get; set; }
		public List<LocationsItem> locations { get; set; }
	}

	public class LocationsItem
	{
		public Coordinates coordinates { get; set; }
		public string country { get; set; }
		public object history { get; set; }
		public List<HistoryRec> HistoryRecs { get; set; }
		public int latest { get; set; }
		public string province { get; set; }

		public LocationsItem()
		{
			HistoryRecs = new List<HistoryRec>();
		}

	}

	public class Coordinates
	{
		public string lat { get; set; }
		public string lon { get; set; }
	}

	public class HistoryRec
	{
		public DateTime dt { get; set; }
		public int Count { get; set; }
	}
}
