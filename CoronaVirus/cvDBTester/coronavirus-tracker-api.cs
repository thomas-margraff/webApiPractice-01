using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace cvDBTester
{
    public class coronavirus_tracker_api
    {
        public coronavirus_tracker_api()
        {

        }

        public void Run()
        {
            GetData().Wait();
        }

        public async Task GetData()
        {
            string jsonData = "";
            string url = "https://coronavirus-tracker-api.herokuapp.com/all";
            using (var client = new HttpClient())
            {
                try
                {
                    jsonData = await client.GetStringAsync(url);
                    ParseData(jsonData);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public void ParseData(string json)
        {
            var cv = JsonConvert.DeserializeObject<dynamic>(json);
            int i = 2;

        }
    }
}
