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

namespace BulkDataOps
{
    class Program
    {
        static void Main(string[] args)
        {
            BulkOps ops = new BulkOps();
            // ops.BulkUpdate();
        }
    }
}
