using CoronaVirusLib.Configuration;
using CoronaVirusLib.Parsers;
using CoronaVirusLib.Messages;
using Newtonsoft.Json;
using RMQLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static System.Console;

namespace CoronaVirusLib.Receivers
{
    public class apiDataReceiver : Receiver
    {
        private int runCount = 0;
        private RabbitContext ctx;

        public apiDataReceiver(RabbitContext ctx):
            base(ctx)
        {
            this.ctx = ctx;
        }

        public void Run()
        {
            base.Register(new CoronaVirusApiTrackerMessage());
            WriteLine("started import receiver");
        }

        public override bool Process(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return true;

            var msg = JsonConvert.DeserializeObject<CoronaVirusApiTrackerMessage>(json);
            
            runCount++;
            ParseApiTrackerModel parser = new ParseApiTrackerModel();
            var cv = parser.Parse(msg.PayloadJson);

            var recs = cv.confirmed.locations.Where(r => r.country == "US")
            .Select(r => new
            {
                r.country,
                r.province,
                r.latest
            }).OrderBy(r => r.country).ThenBy(r => r.province.Trim());

            string dashes = new String('=', 55);
            Clear();
            WriteLine();
            ForegroundColor = ConsoleColor.Green;
            WriteLine("cvApiTracker - Usually 1 day behind...  runCount={0}", runCount);
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


            return true;
        }

    }
}
