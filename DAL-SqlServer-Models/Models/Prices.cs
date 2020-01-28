using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL_SqlServer_Models.Models
{
    public partial class Prices
    {
        [Key]
        public int PriceId { get; set; }
        public int SymbolId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime PriceDateTime { get; set; }
        public double O { get; set; }
        public double H { get; set; }
        public double L { get; set; }
        public double C { get; set; }
        [Column("BO")]
        public double Bo { get; set; }
        [Column("BH")]
        public double Bh { get; set; }
        [Column("BL")]
        public double Bl { get; set; }
        [Column("BC")]
        public double Bc { get; set; }
        [Column("AO")]
        public double Ao { get; set; }
        [Column("AH")]
        public double Ah { get; set; }
        [Column("AL")]
        public double Al { get; set; }
        [Column("AC")]
        public double Ac { get; set; }
    }
}
