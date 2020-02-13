using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CoronaVirusLib.Configuration
{
    public class cvConfig
    {
        private readonly IConfiguration cfg;

        public cvConfig(IConfiguration cfg)
        {
            this.cfg = cfg;
        }

        public string CsvDir()
        {
            return cfg.GetValue<string>("csvDir");
        }
        public string JsonDir()
        {
            return cfg.GetValue<string>("jsonDir");
        }

        public string ConnectionString()
        {
            return cfg.GetValue<string>("connectionString");
        }

        public string GetValue(string val)
        {
            return cfg.GetValue<string>(val);
        }

    }
}
