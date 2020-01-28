using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL_SqlServer_Models.Models
{
    public partial class VwPrices
    {
        [Required]
        [StringLength(6)]
        public string SymbolCode { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime PriceDateTime { get; set; }
        public double O { get; set; }
        public double H { get; set; }
        public double L { get; set; }
        public double C { get; set; }
        public int SymbolId { get; set; }
        public int PriceId { get; set; }
    }
}
