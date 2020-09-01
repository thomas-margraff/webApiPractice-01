using EmailLib;
using Scrappy;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;
using YoutubeExtractor;

namespace ScrapeTester
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // doscrape().Wait();
        }

        static void youtubeDownloader() 
        {
            string link = "https://www.youtube.com/watch?v=0YOVPIQuvAw";
            IEnumerable<VideoInfo> videoInfos = DownloadUrlResolver.GetDownloadUrls(link);

            int i = 2;
            
        }

        static async Task doscrape()
        {
            string html = "";
            var browser = new Browser();
            var page = await browser.Open("https://www.youtube.com/watch?time_continue=4&v=AaLTdrdVcLE&feature=emb_logo");  // https://bnonews.com/index.php/2020/01/the-latest-coronavirus-cases/");
            html = page.Select("#mvp-content-main").Text();

            // The table below shows confirmed cases of coronavirus (2019-nCoV) in China and other countries. To see a distribution map and a timeline, scroll down. 
            // <strong>There are currently 9,171 confirmed cases worldwide, including 213 fatalities.</strong>

            var innerHtml = page.Select("#mvp-content-main").Children()[1].InnerHTML;

            innerHtml = innerHtml.Replace("</strong>", "");
            var parts = innerHtml.Split(">");
            var txt = parts[1];
            Console.WriteLine(txt);
            // { "thomaspmar@hotmail.com" },
            List<string> recipients = new List<string>{
                {"tmargraff.dev@gmail.com"},
                {"tmargraff@gmail.com" }
            };

            Gmail.Send("Corona Virus Update!!", txt, recipients);
            // Gmail.Send(txt, "thomaspmar@hotmail.com");

        }
    }
}
