using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using DAL_SqlServer;
using DAL_SqlServer.Models;
using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Text;
using Microsoft.EntityFrameworkCore;
using DAL_SqlServer.Dto;

namespace BulkDataOps
{
    public class BulkOps
    {
        public void FixBadDates()
        {
            using (var ctx = new ntpContext())
            {
                var dt = new DateTime(2020, 12, 30);
                var recs = (from r in ctx.IndicatorData where r.ReleaseDateTime >= dt select r).ToList();
                foreach (var rec in recs)
                {
                    var exist = ctx.IndicatorData.Where(r => r.EventId == rec.EventId).FirstOrDefault();

                    exist.Actual = rec.Actual;
                    exist.Forecast = rec.Forecast;
                    exist.Indicator = rec.Indicator;
                    exist.Previous = rec.Previous;
                    exist.ReleaseDate = rec.ReleaseDate.Replace("2021", "2020");
                    exist.ReleaseDateTime = rec.ReleaseDateTime.AddYears(-1);
                    exist.ReleaseTime = rec.ReleaseTime;

                    ctx.Entry(exist).State = EntityState.Modified;
                }
                ctx.SaveChanges();
            }
        }

        public void BulkInsertCurrencyIndicatorTable()
        {
            using (var ctx = new ntpContext())
            {
                var recs = (from r in ctx.IndicatorData
                            select new ReleaseDto()
                            {
                                Id = 0,
                                Currency = r.Currency,
                                Indicator = r.Indicator
                            }).Distinct().OrderBy(r => r.Currency).ThenBy(d => d.Indicator).ToList();

                foreach (var rec in recs)
                {
                    var dto = new IndicatorDataCcyName()
                    {
                        CreateDate = DateTime.Now,
                        Currency = rec.Currency,
                        Indicator = rec.Indicator,
                        IsActive = true,
                    };

                    Console.WriteLine(dto.Currency + " " + dto.Indicator);
                    ctx.IndicatorDataCcyName.Add(dto);
                }

                ctx.SaveChanges();

            }

        }
        public void BulkInsert()
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
        public void BulkUpdate()
        {
            string url = "http://localhost:3000/api/scrape/week/this";
            string jsonData = CallRestMethod(url);

            JObject calendar = JObject.Parse(jsonData);
            IList<JToken> results = calendar["calendarData"].Children().ToList();
            IList<IndicatorData> recs = new List<IndicatorData>();
            foreach (JToken result in results)
            {
                // JToken.ToObject is a helper method that uses JsonSerializer internally
                IndicatorData rec = result.ToObject<IndicatorData>();
                recs.Add(rec);
            }

            using (var ctx = new ntpContext())
            {
                foreach (var rec in recs)
                {
                    var exist = ctx.IndicatorData.Where(r => r.EventId == rec.EventId).FirstOrDefault();
                    if (exist == null)
                    {
                        rec.CreateDate = DateTime.Now;
                        ctx.IndicatorData.Add(rec);
                    }
                    else
                    {
                        exist.ModifyDate = DateTime.Now;
                        exist.Actual = rec.Actual;
                        exist.Forecast = rec.Forecast;
                        exist.Indicator = rec.Indicator;
                        exist.Previous = rec.Previous;
                        exist.ReleaseDate = rec.ReleaseDate;
                        exist.ReleaseDateTime = rec.ReleaseDateTime;
                        exist.ReleaseTime = rec.ReleaseTime;

                        ctx.Entry(exist).State = EntityState.Modified;
                    }
                }
                ctx.SaveChanges();
            }

        }

        public string CallRestMethod(string url)
        {
            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(url);
            webrequest.Method = "GET";
            HttpWebResponse webresponse = (HttpWebResponse)webrequest.GetResponse();
            Encoding enc = System.Text.Encoding.GetEncoding("utf-8");
            StreamReader responseStream = new StreamReader(webresponse.GetResponseStream(), enc);
            string result = string.Empty;
            result = responseStream.ReadToEnd();
            webresponse.Close();
            return result;
        }

    }
}
