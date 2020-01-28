using System;
using System.Collections.Generic;
using System.IO;
using Ionic.Zip;
using System.Text;

namespace ForexPriceLib.Utils
{
    public static class Utils
    {
        private static byte[] UnZipToMemory(string LocalCatalogZip)
        {
            var result = new Dictionary<string, MemoryStream>();
            var ret = new MemoryStream();
            using (ZipFile zip = ZipFile.Read(LocalCatalogZip))
            {
                foreach (ZipEntry e in zip)
                {
                    e.Extract(ret);
                }
            }

            return ((MemoryStream)ret).ToArray();
        }

    }
}
