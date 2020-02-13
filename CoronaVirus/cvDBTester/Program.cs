using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace cvDBTester
{
    class Program
    {
        static IConfigurationRoot configuration { get; set; }
        private static IServiceProvider _serviceProvider;

        static void Main(string[] args)
        {
            var startup = new ConsoleStartup();

            configuration = startup.CreateConfig();

            var services = startup.createServices();
            _serviceProvider = services.BuildServiceProvider(true);

            IServiceScope scope = _serviceProvider.CreateScope();
            scope.ServiceProvider.GetRequiredService<DBTester>().Run();
            startup.DisposeServices();
            
        }

        #region readcsvFiles() not used
        //static void readcsvFiles()
        //{
        //    var files = Directory.GetFiles(csvDir).ToList();
        //    var geos = new List<string>();
        //    var geocc = new List<GeoCountry>();

        //    foreach (var file in files)
        //    {
        //        Console.WriteLine(new FileInfo(file).Name);

        //        var lines = (from r in File.ReadAllLines(file).Skip(1) select r);
        //        foreach (var rec in lines)
        //        {
        //            /* 0 - "Mainland China",
        //             * 1 - "2020-02-07T01:10:55.915Z",
        //             * 2 - "Hubei province (including Wuhan)",
        //             * 3 - "22,112",
        //             * 4 - "619*",
        //             * 5 - "3,161 serious, 841 critical"
        //             */

        //            string output = rec.Replace("\",\"", "\"~\"");
        //            var parts = output.Split('~').ToList();
        //            string geoLocation = parts[0].Replace("\"", "").Replace("\"", ""); ;
        //            string country = parts[2].Replace("\"", "").Replace("\"", "");
        //        }
        //    }
        //    var gd = geocc
        //                .Select(m => new { m.GeoLocationName, m.CountryName })
        //                .Distinct()
        //                .ToList();

        //    geos = geos.OrderBy(r => r).Distinct().ToList();

        //    using (var ctx = new CvContext())
        //    {
        //        foreach (var g in geos)
        //        {
        //            var ctys = geocc.Where(r => r.GeoLocationName == g).Select(r => r.CountryName).Distinct().ToList();
        //            ctys = ctys.OrderBy(r => r).ToList();
        //            var rec = ctx.GeoLocations.FirstOrDefault(r => r.Name == g);
        //            foreach (var c in ctys)
        //            {
        //                var cty = new Country();
        //                cty.GeoLocationId = rec.Id;
        //                cty.CreateDate = DateTime.Now;
        //                cty.Name = c;
        //                ctx.Countries.Add(cty);
        //            }
        //        }
        //        ctx.SaveChanges();
        //    }

        //    Console.WriteLine("Done");
        //}
        #endregion readcsvFiles() not used
    }

    //class GeoCountry
    //{
    //    public string GeoLocationName { get; set; }
    //    public string CountryName { get; set; }

    //}

}
