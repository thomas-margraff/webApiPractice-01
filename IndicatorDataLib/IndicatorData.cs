using System;
using System.Net.Http;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DAL_SqlServer.Models;
using Newtonsoft.Json;

namespace IndicatorDataLib
{
    public class NtpIndicatorData
    {
        public async Task<List<IndicatorData>> GetIndicatorsForCcyAndName(string currency, string indicatorName)
        {
            string jsonData = "";
            string baseUrl = "http://localhost:5100/api";

            string url = string.Format("{0}/{1}/{2}/{3}/{4}", baseUrl, "IndicatorData", "GetIndicatorsForCcyAndName", "USD", "Non-Farm Employment Change");
            List<IndicatorData> recs = new List<IndicatorData>();

            using (var client = new HttpClient())
            {
                try
                {
                    jsonData = await client.GetStringAsync(url);
                    recs = JsonConvert.DeserializeObject<List<IndicatorData>>(jsonData);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return recs.ToList(); ;
        }
    }
}
