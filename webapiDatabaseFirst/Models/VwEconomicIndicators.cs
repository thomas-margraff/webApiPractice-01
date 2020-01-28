using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webapiDatabaseFirst.Models
{
    public partial class VwEconomicIndicators
    {
        [Required]
        [StringLength(3)]
        public string CountryCode { get; set; }
        [Required]
        [StringLength(256)]
        public string IndicatorName { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? EventDateTime { get; set; }
        [StringLength(128)]
        public string Actual { get; set; }
        [StringLength(128)]
        public string Forecast { get; set; }
        [StringLength(128)]
        public string Revised { get; set; }
        [StringLength(128)]
        public string Previous { get; set; }
        [StringLength(128)]
        public string Impact { get; set; }
        [Column("EIEId")]
        public int Eieid { get; set; }
        public int IndicatorId { get; set; }
        public int CountryId { get; set; }
    }
}
