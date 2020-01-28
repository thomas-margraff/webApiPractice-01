using ForexPriceLib.FileExtensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ForexPriceLib.Utils
{
    public class SymbolUtils
    {
        public List<string> GetSymbolListFromFiles()
        {
            var file = Directory
                        .GetFiles("I:\\ForexData\\Forexite\\ARCHIVE_PRICES\\ZIP_ORIGINAL", "*.zip")
                        .OrderByDescending(r => r)
                        .ToList().FirstOrDefault();

            var fxp = new ForexPrices();
            fxp.PriceRecords = fxp.PriceRecords.UnzipPrices(file).ToList();
            var symbols = (from r in fxp.Symbols orderby r select r).Distinct().ToList();

            return symbols;
        }
    }
}
