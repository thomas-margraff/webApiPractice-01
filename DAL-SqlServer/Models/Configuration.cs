using System;
using System.Collections.Generic;
using System.Text;

namespace DAL_SqlServer.Models
{
    public class Configuration
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string JsonData { get; set; }

    }
}
