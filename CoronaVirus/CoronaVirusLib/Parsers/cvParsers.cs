using CoronaVirusDAL.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CoronaVirusLib.Configuration;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Scrappy;

namespace CoronaVirusLib.Parsers
{
    public class cvParsers
    {
        private readonly cvConfig cvConfiguration;

        public cvParsers(cvConfig cvConfiguration)
        {
            this.cvConfiguration = cvConfiguration;
        }

        public List<string> GetJsonFiles()
        {
            return Directory.GetFiles(cvConfiguration.JsonDir()).ToList();
        }

        public cvScrapeData ParseJson()
        {
            var files = Directory.GetFiles(cvConfiguration.JsonDir()).ToList();
            string json = File.ReadAllText(files.FirstOrDefault());
            var data = new cvScrapeData();
            data = JsonConvert.DeserializeObject<cvScrapeData>(json);

            return data;
        }

        public cvScrapeData ParseJson(string file)
        {
            string json = File.ReadAllText(file);
            var data = new cvScrapeData();
            data = JsonConvert.DeserializeObject<cvScrapeData>(json);
            return data;
        }

        public void ParseHtmlFile()
        {
            string filePath = @"C:\devApps\typescriptscraper\coronavirus\data\html\2020.02.12-1642-bno.html";
            var page = WebPage.CreateFromFile(filePath);
            string heading = page.Select("#mvp-content-main").Text();
            var selector = "#mvp-content-main > table.wp-block-table.aligncenter.is-style-stripes > tbody > tr";
            var tbl1 = page.Select(selector).Text();

        }
    }
}
