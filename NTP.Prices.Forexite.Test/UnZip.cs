using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Compression.Zipstorer;
using System.IO;

namespace NTP.Prices.Forexite.Test
{
    public static class UnZip
    {
        public static string FileToString(string fileName)
        {
            ZipStorer zip = ZipStorer.Open(fileName, System.IO.FileAccess.Read);

            // Read the central directory collection
            List<ZipStorer.ZipFileEntry> dir = zip.ReadCentralDir();

            if (dir.Count == 0)
                throw new Exception("no files in " + fileName);

            byte[] block = new byte[0];
            using (var ms = new MemoryStream())
            {
                zip.ExtractFile(dir[0], ms);
                block = ms.ToArray();
            }

            string recs = System.Text.Encoding.Default.GetString(block, 0, block.Length);
            zip.Close();

            return recs;
        }

    }
}
