using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using ForexPriceLib.Models;
using ForexPriceLib.Utils;
using ForexPriceLib.FileExtensions;
using System.Net.Http;
using System.Threading.Tasks;
using IndicatorDataLib;
using DAL_SqlServer.Models;
using Newtonsoft.Json;
using DAL_SqlServer.ModelExtensions;

namespace NTP.Prices.Forexite.Test
{

    /* 
     * L:\wincorp-quad-01\NewstraderPro\_NTP.3.0\ClassLibraries\NTP.DomainObjects.Extensions
     */

    class Program
    {
        static AppConfig appConfig;
        async static Task Main(string[] args)
        {
            #region config
            var builder = new ConfigurationBuilder()
                           .SetBasePath(Directory.GetCurrentDirectory())
                           .AddJsonFile("appsettings.json");

            var config = builder.Build();
            appConfig = config.GetSection("AppConfiguration").Get<AppConfig>();

            #endregion

            var fxp = new ForexPrices();
            fxp.Read(DateTime.Now.AddDays(-2));
            var x = fxp.PriceRecords.ToSymbols();

            // var y = fxp.SymbolTimeStats.ToCsv();
            
            int i = 0;

            //var csv = fxp.SymbolTimeStats.ToCsv();
            //csv.Insert(0, ForexTimePeriodStats.ToTimeStatCsvHeader());
            //int i = 0;
            //File.WriteAllLines("ts.csv", csv.ToArray());

            // var s = new SymbolUtils();
            // s.GetSymbolListFromFiles();

            // Downloader dl = new Downloader();
            // dl.DownloadMissingPriceFiles();

            // await getNfpTimeStats();
            // await getIndicators();
            // jsonPriceTests();

            wait();
        }

        static async Task getNfpTimeStats()
        {
            var ntpIndicatorData = new NtpIndicatorData();
            List<IndicatorData> recs = await ntpIndicatorData.GetIndicatorsForCcyAndName("USD", "Non-Farm Employment Change");
            List<DateTime> dts = new List<DateTime>();

            foreach (var rec in recs)
            {
                var dt = DateTime.Parse(rec.ReleaseDate + " " + rec.ReleaseTime).AddHours(6).AddMinutes(1);
                dts.Add(dt);
            }

            SymbolTimeStatsUtils utils = new SymbolTimeStatsUtils();
            var tstats = utils.GetTimeStats(dts);

            var tJson = (from r in tstats
                         select new
                         {
                             r.Symbol,
                             r.DateTimeBeginAnchor,
                             r.DateTimeBegin,
                             r.DateTimeEnd,
                             r.DateTimeEndAnchor,
                             r.Open,
                             r.High,
                             r.Low,
                             r.Close,
                             r.PipMultiplier,
                             r.PipsHigh,
                             r.PipsLow,
                             r.PipsRange
                         }).ToList();

            //foreach (var r in tJson)
            //{
            //    var x = string.Format("{0}",
            //                 r.Symbol,
            //                 r.DateTimeBeginAnchor,
            //                 r.DateTimeBegin,
            //                 r.DateTimeEnd,
            //                 r.DateTimeEndAnchor,
            //                 r.Open,
            //                 r.High,
            //                 r.Low,
            //                 r.Close,
            //                 r.PipMultiplier,
            //                 r.PipsHigh,
            //                 r.PipsLow,
            //                 r.PipsRange);
            //}

            var jsonTimeStats = JsonConvert.SerializeObject(tJson, Formatting.Indented);
            // var jsonTimeStats = tstats.ToJson();
            File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "nfpTimeStats.json"), jsonTimeStats);
        }


        static async Task getIndicators()
        {
            var ntpIndicatorData = new NtpIndicatorData();
            List<IndicatorData> recs = await ntpIndicatorData.GetIndicatorsForCcyAndName("USD", "Non-Farm Employment Change");

            List<string> csvList = new List<string>();
            List<string> csvStats = new List<string>();
            csvStats.Add(ForexTimePeriodStats.ToTimeStatCsvHeader());
            foreach (var rec in recs)
            {
                // Console.WriteLine(rec.ReleaseDateTime.AddHours(2));
                var dt = DateTime.Parse(rec.ReleaseDate + " " + rec.ReleaseTime).AddHours(6).AddMinutes(1);
                rec.ReleaseDateTime = dt;
                Console.WriteLine(rec.ReleaseDateTime);

                var fxp = new ForexPrices();
                try
                {
                    fxp.Read(dt, "EURUSD");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    continue;
                }
                fxp.PriceRecords = fxp.PriceRecords
                    .Where(r => r.PriceDateTime.TimeOfDay >= rec.ReleaseDateTime.TimeOfDay && r.PriceDateTime.TimeOfDay <= rec.ReleaseDateTime.AddMinutes(180).TimeOfDay)
                    .ToList();
                Console.WriteLine("price recs: " + fxp.PriceRecords.Count());
                Console.WriteLine("");
                //csvList.AddRange(fxp.PriceRecords.ToCsvListOHLC());
                //csvList.Add("");
                if (fxp.SymbolTimeStats.Count() > 0)
                {
                    csvStats.AddRange(fxp.SymbolTimeStats.ToCsv());
                }
                else
                {
                    Console.WriteLine("no recs");
                }
            }

            // File.WriteAllLines("nfpList.csv", csvList.ToArray());
            File.WriteAllLines("nfpListTimeStats.csv", csvStats.ToArray());
        }

        static void jsonPriceTests()
        {
            DateTime dt = new DateTime(2019, 7, 5);

            var fxp = new ForexPrices();
            fxp.Read(dt);
            fxp.PriceRecords = fxp.PriceRecords
                .Where(r => r.PriceDateTime.TimeOfDay >= new TimeSpan(14, 31, 0) && r.PriceDateTime.TimeOfDay <= new TimeSpan(15, 31, 0))
                .ToList();

            var json = fxp.ToJson();
            var jsonTimeStats = fxp.SymbolTimeStats.ToJson();
            var jsonPrices = fxp.PriceRecords.ToJson();

            // File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "fxp.json"), json);
            // File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "fxpTimeStats.json"), jsonTimeStats);
            File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "fxpPriceRecs.json"), jsonPrices);

            fxp.PriceRecords.ToCsvFileOHLC(Path.Combine(Directory.GetCurrentDirectory(), "fxp.csv"), true);
            Console.WriteLine(fxp.PriceRecords.Count());
        }

        static void getSomeRecs()
        {
            var dtStart = new DateTime(2019, 10, 1);
            var dtEnd = new DateTime(2019, 12, 13);

            while (dtStart <= dtEnd)
            {
                if (dtStart.DayOfWeek != DayOfWeek.Saturday)
                {
                    getSymbolPrices("EURUSD", dtStart);
                }
                dtStart = dtStart.AddDays(1);
            }
        }

        static void getSymbolPrices(string symbol, DateTime dt)
        {
            var fpath = Path.Combine(appConfig.ZipPath, FileUtils.DateToFxiFileName(dt, "zip"));
            var recs = new List<ForexPriceRecord>()
                        .UnzipPrices(fpath)
                        .OrderByDescending(r => r.PipsRange)
                        .Where(r => r.Symbol == symbol).ToList();

            Console.WriteLine(recs.FirstOrDefault().ToString());
        }

        static void splitSymbols()
        {
            var files = Directory.GetFiles(appConfig.ZipPath, "*.zip").ToList();
            var symbolPricesList = new List<ForexSymbolPrices>();

            foreach (var file in files)
            {
                var finfo = new FileInfo(file);

                var recs = new List<ForexPriceRecord>().UnzipPrices(file);
                recs = recs.OrderBy(r => r.Symbol).ThenBy(r => r.PriceDateTime).ToList();
                var symbols = recs.OrderBy(r => r.Symbol).Select(r => r.Symbol).Distinct();
                foreach (var symbol in symbols)
                {
                    var symbolPrices = new ForexSymbolPrices();
                    symbolPrices.Symbol = symbol;
                    symbolPrices.Prices = recs.Where(r => r.Symbol == symbol).ToList();
                    var fxp = new ForexPrices();
                    fxp.PriceRecords = recs;

                    int z = 2;
                    // symbolPricesList.Add(symbolPrices);
                }
                Console.WriteLine(finfo.Name);
            }
        }

        static void unZipAll()
        {
            var files = Directory.GetFiles(appConfig.ZipPath, "*.zip").ToList();
            foreach (var file in files)
            {
                try
                {
                    var finfo = new FileInfo(file);
                    var csvName = finfo.Name.Replace(".zip", ".csv");
                    Console.WriteLine(csvName);
                    var inFile = Path.Combine(appConfig.ZipPath, finfo.Name);
                    var outFile = Path.Combine(appConfig.CsvPath, csvName);
                    if (!File.Exists(outFile))
                    {
                        new List<ForexPriceRecord>().UnzipPrices(inFile).ToCsvFileOHLC(outFile);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        static void wait()
        {
            Console.WriteLine();
            Console.WriteLine("done");
            Console.ReadLine();
        }
    }
}
