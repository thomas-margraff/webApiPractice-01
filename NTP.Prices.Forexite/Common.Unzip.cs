using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Compression;
using System.IO;

namespace Wincorp.Common.Compression
{
    public class UnZip
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

        public static List<string> ToLocalFile(string zipFile, string unzipFilePath)
        {
            List<string> fileNames = new List<string>();
            ZipStorer zip = ZipStorer.Open(zipFile, FileAccess.Read);
            
            // Read the central directory collection
            List<ZipStorer.ZipFileEntry> dir = zip.ReadCentralDir();
            foreach (ZipStorer.ZipFileEntry entry in dir)
            {
                fileNames.Add(entry.FilenameInZip);
                zip.ExtractFile(entry, Path.Combine(unzipFilePath, entry.FilenameInZip));
            }
            zip.Close();

            return fileNames;
        }
    }
}
