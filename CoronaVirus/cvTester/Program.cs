using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CoronaVirusDAL;

namespace cvTester
{
    class Program
    {
        static string baseDir = @"C:\devApps\typescriptscraper\coronavirus\data";
        static string csvDir = Path.Combine(baseDir, "csv");
        static string jsonDir = Path.Combine(baseDir, "json");

        static void Main(string[] args)
        {
            readcsvFiles();
        }

        static void readcsvFiles()
        {
            var files = Directory.GetFiles(csvDir).ToList();
            var allCsv = new List<string>();
            foreach (var file in files)
            {
                Console.WriteLine(new FileInfo(file).Name);

                var lines = (from r in File.ReadAllLines(file).Skip(1) select r);
                foreach (var rec in lines)
                {
                    /* "Mainland China",
                     * "2020-02-07T01:10:55.915Z",
                     * "Hubei province (including Wuhan)",
                     * "22,112",
                     * "619*",
                     * "3,161 serious, 841 critical"
                     */

                    string output = rec.Replace("\",\"", "\"~\"");

                    var parts = output.Split('~').ToList();

                    var newCsv = string.Format("{0},{1},{2},{3},{4}", parts[0], parts[2], parts[3], parts[4], parts[5]);

                    allCsv.Add(newCsv);
                }
            }
            allCsv = allCsv.Distinct().ToList();
            Console.WriteLine("Done");
        }
    }
}
