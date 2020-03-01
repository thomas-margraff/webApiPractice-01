using ForexPriceLib.Utils;
using System;
using System.IO;
using System.Linq;
using System.Net;
using EasyNetQ;
using EasyNetQ.Common.Connection;
using EasyNetQ.Common.Messages;
using static System.Console;
using System.Threading.Tasks;
using EasyNetQ.Common.Messages.Download;

namespace SendDownload
{
    class Program
    {
        static string forexiteFolder = @"I:\ForexData\Forexite\ARCHIVE_PRICES\ZIP_ORIGINAL";
        static string forexiteBaseUrl = "https://www.forexite.com/free_forex_quotes/";
        static string forexiteSaveToPath = @"C:\devApps\webApiPractice-01\EasyNetQ\Forexite\ZipFiles";

        static void Main(string[] args)
        {
            var lastDownloadDate = getLastScrape();
            var nextDownloadDate = getNextDownloadDate(lastDownloadDate);

            // temp - file not ready yet
            nextDownloadDate = nextDownloadDate.AddDays(-1);
            string url = Path.Combine(forexiteBaseUrl, FileUtils.DateToUrlFileName(nextDownloadDate) + ".zip");
            byte[] fileBytes = webClientDownloadToBytesAsync(url).Result;

            ForexiteDownloadMessage msg = new ForexiteDownloadMessage()
            {
                DownloadDataForDate = nextDownloadDate,
                FileBytes = fileBytes,
                Url = url
            };

            using (var bus = RabbitHutch.CreateBus(Connection.Local()))
            {
                bus.Send("download.forexite", msg);
            }

            int i = 0;
        }

        static async Task<byte[]> webClientDownloadToBytesAsync(string url)
        {
            try
            {
                WebClient client = new WebClient();
                var data = await client.DownloadDataTaskAsync(new Uri(url));
                return data;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("404"))
                    return null;

                throw;
            }
        }

        static byte[] webClientDownloadToBytes(string url)
        {
            byte[] fileBytes;

            using (WebClient client = new WebClient())
            {
                try
                {
                    fileBytes = client.DownloadData(new Uri(url));
                    return fileBytes;
                }
                catch (Exception ex)
                {
                    WriteLine(ex.Message);
                }
            }
            return new byte[0];
        }

        static void webClientDownloadToFile(string url, string fname)
        {
            using (WebClient client = new WebClient())
            {
                try
                {
                    client.DownloadFile(new Uri(url), Path.Combine(forexiteSaveToPath, fname));
                }
                catch (Exception ex)
                {
                    WriteLine(ex.Message);
                }
            }

        }

        static DateTime getNextDownloadDate(DateTime dt)
        {
            var dtNext = dt.AddDays(1);
            if (dtNext.DayOfWeek == DayOfWeek.Saturday)
                dtNext = dt.AddDays(1);

            return dtNext;
        }

        static DateTime getLastScrape()
        {
            var file = Directory.GetFiles(forexiteFolder).OrderByDescending(r => r).FirstOrDefault();
            var dt = FileUtils.FileNameToDate(file);
            return dt;
        }

    }
}
