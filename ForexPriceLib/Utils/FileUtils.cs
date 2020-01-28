using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ForexPriceLib.Utils
{
    public class FileUtils
    {
        public static string DateToFxiFileName(DateTime dt, string ext)
        {
            // FXIT-20200103.csv 
            // FXIT-20200103.zip
            string yyyy = dt.Year.ToString();
            string yy = yyyy.Substring(2, 2);
            string mm = dt.Month.ToString();
            if (mm.Length == 1) mm = "0" + mm;
            string dd = dt.Day.ToString();
            if (dd.Length == 1) dd = "0" + dd;

            var fname = string.Format("FXIT-{0}{1}{2}.{3}", yyyy, mm, dd, ext);
            return fname;
        }

        public static string DateToFileName(DateTime dt, string ext)
        {
            // FXIT-20200103.csv 
            return Path.Combine(dt.ToString("yyyy"), string.Format("{0}.{1}.{2}{3}{4}",
                dt.Year,
                dt.Month.ToString().PadLeft(2, '0'),
                dt.Day.ToString().PadLeft(2, '0'),
                dt.Month.ToString().PadLeft(2, '0'),
                dt.Year.ToString().Substring(2, 2)) + ext); // ".zip");
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
