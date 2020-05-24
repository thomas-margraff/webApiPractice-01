using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RMQLib
{
    public static class RMQContextEx
    {
        public static RMQContext Create(this RMQContext ctx, string fileName)
        {
            ctx = new RMQContext();

            var filePath = Path.Combine(@"Configuration", fileName);
            string fullFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filePath);
            string json = File.ReadAllText(fullFilePath);

            ctx = JsonConvert.DeserializeObject<RMQContext>(json);

            return ctx;
        }
    }
}
