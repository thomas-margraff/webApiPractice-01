using DAL_SqlServer.Models;
using ForexPriceLib.FileExtensions;
using ForexPriceLib.Utils;
using IndicatorDataLib;
using NtpDataLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NtpDataLib.Extensions
{
    public static class IndicatorPricesEx
    {

        public static async Task<IndicatorPrices> Load(this IndicatorPrices indicatorPrices, string country, string indicator)
        {
            var ntpIndicatorData = new NtpIndicatorData();

            List<IndicatorData> recs = new List<IndicatorData>();
            recs = await ntpIndicatorData.GetIndicatorsForCcyAndName("USD", "Non-Farm Employment Change");

            indicatorPrices = new IndicatorPrices();
            indicatorPrices.Currency = "USD";
            indicatorPrices.Indicator = "Non-Farm Employment Change";

            indicatorPrices.IndicatorDetails = (from r in recs
                                                select new IndicatorDetail
                                                {
                                                    Actual = r.Actual,
                                                    EventId = r.EventId,
                                                    Forecast = r.Forecast,
                                                    Id = r.Id,
                                                    Previous = r.Previous,
                                                    ReleaseDate = r.ReleaseDate,
                                                    ReleaseDateTime = DateTime.Parse(r.ReleaseDate + " " + r.ReleaseTime).AddHours(6).AddMinutes(1), // r.ReleaseDateTime,
                                                    ReleaseTime = r.ReleaseTime
                                                }).ToList(); ;

            foreach (var rec in indicatorPrices.IndicatorDetails)
            {
                var fxp = new ForexPrices();
                fxp.Read(rec.ReleaseDateTime.Date);
                fxp.PriceRecords = fxp.PriceRecords
                    .Where(r => r.PriceDateTime.TimeOfDay >= rec.ReleaseDateTime.TimeOfDay && r.PriceDateTime.TimeOfDay <= rec.ReleaseDateTime.AddMinutes(180).TimeOfDay)
                    .ToList();
                
                foreach (var symbol in fxp.Symbols)
                {
                    IndicatorDetailPrices idp = new IndicatorDetailPrices();
                    idp.Symbol = symbol;
                    idp.PricesDetail = (from r in fxp.PriceRecords where r.Symbol == symbol select new IndicatorDetailPricesDetail 
                    { 
                        Close = r.Close,
                        High= r.High,
                        Low=r.Low,
                        Open = r.Open,
                        PriceDateTime = r.PriceDateTime.DateTime
                    }).ToList();
                    rec.IndicatorDetailPrices.Add(idp);
                }
                
                // rec.ForexPrices.Add(fxp);

                Console.WriteLine(rec.ReleaseDateTime.Date);
            }
            return indicatorPrices;
            
        }
    }
}
