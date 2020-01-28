using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DAL_SqlServer.Models
{
    public partial class Symbols
    {
        public Symbols()
        {
            // Prices = new HashSet<Prices>();
        }

        [Key]
        public int SymbolId { get; set; }
        public string SymbolCode { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreate { get; set; }
        public DateTime? DateModify { get; set; }

        // public virtual ICollection<Prices> Prices { get; set; }
    }
}
