using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using DAL_SqlServer;
using DAL_SqlServer.Models;
using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BulkDataOps
{
    public class BulkOps
    {
        public void Load()
        {
            var path = @"C:\____projecrts\___dev_scrapers\data\jsondata";
            var files = Directory.GetFiles(path, "*.json");

            foreach (var file in files)
            {
                var jsonData = File.ReadAllText(file);
                FileInfo finfo = new FileInfo(file);
                Console.Write(finfo.Name);

                JObject calendar = JObject.Parse(jsonData);
                IList<JToken> results = calendar["calendarData"].Children().ToList();
                IList<IndicatorData> recs = new List<IndicatorData>();
                foreach (JToken result in results)
                {
                    // JToken.ToObject is a helper method that uses JsonSerializer internally
                    IndicatorData rec = result.ToObject<IndicatorData>();
                    rec.CreateDate = DateTime.Now;
                    recs.Add(rec);
                }
                Console.Write(" recs:" + recs.Count());

                using (var ctx = new ntpContext())
                {
                    foreach (var rec in recs)
                    {
                        ctx.IndicatorData.Add(rec);
                    }
                    ctx.SaveChanges();
                }
                Console.WriteLine("  saved!");
                Console.WriteLine();
            }

        }
    
        public void Update(string jsonData)
        {

        }
    
    }
}
