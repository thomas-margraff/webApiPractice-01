using CoronaVirusLib.Parsers;
using EasyNetQ;
using CoronaVirusLib.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static System.Console;

namespace coronavirus.tracker.api
{
    public class EasyNetQReceiver
    {
		private int runCount = 0;

        public void Subscribe()
        {

			using (var bus = RabbitHutch.CreateBus(Connection.CloudAmqp()))
            {
                bus.Subscribe<CoronaVirusApiTrackerMessage>("cvApiTracker", message => HandleApiMessage(message));
				
                Console.WriteLine("Listening for cvApiTracker messages. Hit <return> to quit.");
                Console.ReadLine();
            }
        }
        private void HandleApiMessage(CoronaVirusApiTrackerMessage msg)
        {
			runCount++;

			ParseApiTrackerModel parser = new ParseApiTrackerModel();
			var cv = parser.Parse(msg.PayloadJson);

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
			WriteLine("cvApiTracker - Usually 1 day behind...  runCunt={0}", runCount);
			WriteLine("As of {0}", DateTime.Now.ToString("MM.dd.yyyy hh:mm ss"));
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
