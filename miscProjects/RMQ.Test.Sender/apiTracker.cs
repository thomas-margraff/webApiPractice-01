using CoronaVirusLib.ApiTracker;
using CoronaVirusLib.Messages;
using CoronaVirusLib.Parsers;
using Newtonsoft.Json;
using RMQLib;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Console;

namespace RMQ.Test.Sender
{
    public class apiTracker
    {
        private RmqSender _sender;
        private RabbitContext _ctx;
        private RMQSession _session;
        private apiTrackerReceiver _receiver;

        public apiTracker()
        {
            _session = new RMQSession("rmq.config.json");
            _session.Create("local", "cv.messages.exchange", "cv.messages.queue", "cvApiTracker");

            _receiver = new apiTrackerReceiver(_session.RabbitContext);
        }

        public void Run()
        {
            while (true)
            {
                Console.WriteLine("(apiTrackerSender) press enter to send another message...");
                Console.ReadLine();
                CoronaVirusApiTrackerMessage msg = CallApi().Result;
                _session.Sender.Send(msg);

                Thread.Sleep(TimeSpan.FromSeconds(30));
            }
        }
        private async Task<CoronaVirusApiTrackerMessage> CallApi()
        {
            var api = new GetDataFromApi();
            string json = await api.GetJsonData();

            var msg = new CoronaVirusApiTrackerMessage();
            msg.PayloadJson = json;

            return msg;
        }
    }

    public class apiTrackerReceiver : RMQReceiver
    {
        private int runCount = 0;
        public apiTrackerReceiver(RabbitContext ctx) : base(ctx)
        {
            Register(ctx.Queue.Name, "cvApiTracker");
        }


        public override bool Process(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return true;

            var msg = JsonConvert.DeserializeObject<CoronaVirusApiTrackerMessage>(json);

            if (msg.PayloadJson == null)
                return true;

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
                state = "st", // r.province.Replace(r.province.Split(',')[0], "").Replace(",", "").Trim(),
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

        //public override bool Process(string json)
        //{
        //    runCount++;
        //    var msg = JsonConvert.DeserializeObject<CoronaVirusApiTrackerMessage>(json);
        //    ParseApiTrackerModel parser = new ParseApiTrackerModel();
        //    var cv = parser.Parse(msg.PayloadJson);

        //    json = JsonConvert.SerializeObject(cv);
        //    json = json.Replace(@"\", "");

        //    var recs = cv.confirmed.locations.Where(r => r.country == "US")
        //        .Select(r => new
        //        {
        //            r.country,
        //            state = r.province.Replace(r.province.Split(',')[0], "").Replace(",", "").Trim(),
        //            r.latest
        //        }).OrderBy(r => r.country).ThenBy(r => r.state.Trim()).ToList();

        //    var states = recs.OrderBy(r => r.state).Distinct();
        //    foreach (var state in states)
        //    {
        //        WriteLine($"{state} ");
        //    }

        //    StringBuilder sb = new StringBuilder();

        //    string dashes = new String('=', 55);
        //    Clear();
        //    WriteLine();
        //    ForegroundColor = ConsoleColor.Green;

        //    sb.AppendFormat("cvApiTracker - Usually 1 day behind...  runCount={0}", runCount).AppendLine();
        //    sb.AppendFormat("As of {0}", DateTime.Now.ToString("MM.dd.yyyy hh:mm ss")).AppendLine();
        //    ForegroundColor = ConsoleColor.White;

        //    sb.AppendLine(dashes);
        //    sb.AppendFormat($"{"State Location".PadRight(45)} Latest").AppendLine();
        //    sb.AppendLine(dashes);

        //    int latest = 0;
        //    foreach (var rec in recs)
        //    {
        //        latest += rec.latest;
        //        //sb.AppendFormat($"{ rec.state } {rec.location.PadRight(50)} {rec.latest}").AppendLine();
        //    }
        //    sb.AppendLine(dashes);
        //    sb.AppendFormat($"{"Total".PadRight(50)} {recs.Sum(r => r.latest)}").AppendLine();
        //    sb.AppendLine(dashes);

        //    WriteLine(sb.ToString());

        //    return true;
        //}
    }

}
