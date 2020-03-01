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

        public static string DateToZipFileName(DateTime dt)
        {
            return string.Format("{0}.{1}.{2}{3}{4}",
                dt.Year,
                dt.Month.ToString().PadLeft(2, '0'),
                dt.Day.ToString().PadLeft(2, '0'),
                dt.Month.ToString().PadLeft(2, '0'),
                dt.Year.ToString().Substring(2, 2)) + ".zip";
        }

        public static string DateToUrlFileName(DateTime dt)
        {
            string yyyy = dt.Year.ToString();
            string yy = yyyy.Substring(2, 2);
            string mm = dt.Month.ToString();
            if (mm.Length == 1) mm = "0" + mm;
            string dd = dt.Day.ToString();
            if (dd.Length == 1) dd = "0" + dd;

            var urlDt = string.Format("{0}/{1}/{2}{3}{04}", yyyy, mm, dd, mm, yy);
            return urlDt;
        }

        public static DateTime FileNameToDate(string filePath)
        {
            DateTime dt = DateTime.Now;
            var finfo = new FileInfo(filePath);
            var fname = finfo.Name.Replace("FXIT-", "").Substring(0, 8);
            var yyyy = Convert.ToInt16(fname.Substring(0, 4));
            var mm = Convert.ToInt16(fname.Substring(4, 2));
            var dd = Convert.ToInt16(fname.Substring(6, 2));

            dt = new DateTime(yyyy, mm, dd);

            return dt;
        }
    }
}
