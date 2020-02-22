using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TestLib.Models;

namespace TestLib
{
    public static class RabbitContextExtensions
    {
        public static RabbitContext Create(this RabbitContext ctx, string fileName)
        {
            ctx = new RabbitContext();

            var filePath = Path.Combine(@"Configuration", fileName);
            string fullFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filePath);
            string json = File.ReadAllText(fullFilePath);
            ctx = JsonConvert.DeserializeObject<RabbitContext>(json);
            return ctx;
        }
    }
}
