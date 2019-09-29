using System;
using System.Collections.Generic;

namespace DAL_SqlServer
{
    public partial class Symbols
    {
        public Symbols()
        {
            Prices = new HashSet<Prices>();
        }

        public int SymbolId { get; set; }
        public string SymbolCode { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<Prices> Prices { get; set; }
    }
}
