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
    public class apiDataReceiver : RMQReceiver
    {
        private int runCount = 0;
        private RabbitContext ctx;

        public apiDataReceiver(RabbitContext ctx) :
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
            json = JsonConvert.SerializeObject(cv);
            json = json.Replace(@"\", "");

            var recs = cv.confirmed.locations.Where(r => r.country == "US")
            .Select(r => new
            {
                r.country,
                location = r.province.Split(',')[0],
                state = r.province.Replace(r.province.Split(',')[0], "").Replace(",", "").Trim(),
                //r.province,
                r.latest
            }).OrderBy(r => r.country).ThenBy(r => r.state.Trim()).ThenBy(r => r.location);
            StringBuilder sb = new StringBuilder();

            string dashes = new String('=', 55);
            Clear();
            WriteLine();
            ForegroundColor = ConsoleColor.Green;

            sb.AppendFormat("cvApiTracker - Usually 1 day behind...  runCount={0}", runCount).AppendLine();
            sb.AppendFormat("As of {0}", DateTime.Now.ToString("MM.dd.yyyy hh:mm ss")).AppendLine();
            ForegroundColor = ConsoleColor.White;

            sb.AppendLine(dashes);
            sb.AppendFormat($"{"State Location".PadRight(45)} Latest").AppendLine();
            sb.AppendLine(dashes);

            int latest = 0;
            foreach (var rec in recs)
            {
                latest += rec.latest;
                sb.AppendFormat($"{ rec.state } {rec.location.PadRight(50)} {rec.latest}").AppendLine();
            }
            sb.AppendLine(dashes);
            sb.AppendFormat($"{"Total".PadRight(50)} {recs.Sum(r => r.latest)}").AppendLine();
            sb.AppendLine(dashes);

            WriteLine(sb.ToString());
            return true;
        }

    }
}
