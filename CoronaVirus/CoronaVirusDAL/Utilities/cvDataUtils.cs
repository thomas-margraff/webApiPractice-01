using CoronaVirusDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoronaVirusDAL.Utilities
{
    public class cvDataUtils
    {
        private static CvContext ctx = new CvContext();

        public static void createGeolocationsFromScrapeFile(cvScrapeData data)
        {
            foreach (var geo in data.geoLocations)
            {
                var rec = ctx.GeoLocations.FirstOrDefault(r => r.Name == geo.name);
                if (rec == null)
                {
                    var g = new GeoLocation()
                    {
                        CreateDate = DateTime.Now,
                        Name = geo.name
                    };
                    ctx.GeoLocations.Add(g);
                    ctx.SaveChanges();
                }
            }
        }

        public static void createCountriesFromScrapeFile(cvScrapeData data)
        {
            foreach (var geo in data.geoLocations)
            {
                var rec = ctx.GeoLocations.FirstOrDefault(r => r.Name == geo.name);
                if (rec == null)
                {
                    throw new Exception("no geolocation record for: " + geo.name);
                }
                // Console.WriteLine("geo {0}", geo.name);
                foreach (var cc in geo.details)
                {
                    var countryRec = ctx.Countries.FirstOrDefault(r => r.Name == cc.country);
                    if (countryRec == null)
                    {
                        if (cc.country != "TOTAL")
                        {
                            Console.WriteLine(" add country ", cc.country);
                            var country = new Country()
                            {
                                CreateDate = DateTime.Now,
                                GeoLocationId = rec.Id,
                                Name = cc.country
                            };
                            ctx.Countries.Add(country);
                            ctx.SaveChanges();
                        }
                    }
                }
            }

        }


    }
}
