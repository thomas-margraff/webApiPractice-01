using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace NTP.Prices.Forexite
{
    public class Forexite
    {
        public static DateTimeOffset ParseDate(string date, string time)
        {
            //       01234567
            // date="20090121"
            // time="000100"
            // time="235900"
            // time="hhmm00"
            DateTime dt = new DateTime(
                int.Parse(date.Substring(0, 4)),
                int.Parse(date.Substring(4, 2)),
                int.Parse(date.Substring(6, 2)))
                .Add(new TimeSpan(int.Parse(time.Substring(0, 2)), int.Parse(time.Substring(2, 2)), 0));

            DateTimeOffset dto = new DateTimeOffset(dt, new TimeSpan(-1, 0, 0));
            return dto;
        }

        public static string DateToFileName(DateTime dt)
        {
            return Path.Combine(dt.ToString("yyyy"),string.Format("{0}.{1}.{2}{3}{4}",
                dt.Year,
                dt.Month.ToString().PadLeft(2, '0'),
                dt.Day.ToString().PadLeft(2, '0'),
                dt.Month.ToString().PadLeft(2, '0'),
                dt.Year.ToString().Substring(2, 2)) + ".zip");
        }

        public static string DateToFileName1(DateTime dt)
        {
            return string.Format("{0}.{1}.{2}{3}{4}",
                dt.Year,
                dt.Month.ToString().PadLeft(2, '0'),
                dt.Day.ToString().PadLeft(2, '0'),
                dt.Month.ToString().PadLeft(2, '0'),
                dt.Year.ToString().Substring(2, 2)) + ".zip";
        }

    }
}
