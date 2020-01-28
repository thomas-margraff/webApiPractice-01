using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;

namespace Wincorp.Common.Compression
{
    public class ZipFileHelper
    {
        public static void WriteZipFile(string filenameZip, string internalFileName, List<string> recs, string comment)
        {
            StringBuilder sb = new StringBuilder();
            recs.ForEach(r => sb.AppendLine(r));
            WriteZipFile(filenameZip, internalFileName, sb.ToString(), comment);
        }

        public static void WriteZipFile(string filenameZip, string fileToZip)
        {
            WriteZipFile(filenameZip, fileToZip, string.Empty);
        }
        public static void WriteZipFile(string filenameZip, string fileToZip, string comment)
        {
            string internalFilename = new FileInfo(fileToZip).Name;
            using (ZipStorer zip = ZipStorer.Create(filenameZip, ""))
            {
                zip.AddFile(ZipStorer.Compression.Deflate, fileToZip, internalFilename, comment);
            }
        }

        public static void WriteZipFile(string filenameZip, string internalFileName, string text, string comment)
        {
            MemoryStream csvBytes = new MemoryStream(Encoding.UTF8.GetBytes(text));
            using (ZipStorer zip = ZipStorer.Create(filenameZip, ""))
            {
                zip.AddStream(ZipStorer.Compression.Deflate, internalFileName, csvBytes, DateTime.Now, comment);
            }
        }

        public static MemoryStream WriteZipFileToMemory(string internalFileName, List<string> recs, string comment)
        {
            StringBuilder sb = new StringBuilder();
            recs.ForEach(r => sb.AppendLine(r));
            return WriteZipFileToMemory(internalFileName, sb.ToString(), comment);
        }

        public static MemoryStream WriteZipFileToMemory(string internalFileName, string text, string comment)
        {
            MemoryStream csvBytes = new MemoryStream(Encoding.UTF8.GetBytes(text));
            ZipStorer zip = ZipStorer.Create(csvBytes, "");
            zip.AddStream(ZipStorer.Compression.Deflate, internalFileName, csvBytes, DateTime.Now, comment);
            var ms = new MemoryStream();
            List<ZipStorer.ZipFileEntry> dir = zip.ReadCentralDir();
            zip.ExtractFile(dir[0], ms);
            return ms;
        }
    }
}
