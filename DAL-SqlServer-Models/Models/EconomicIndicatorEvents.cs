using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL_SqlServer_Models.Models
{
    public partial class EconomicIndicatorEvents
    {
        [Key]
        [Column("EIEId")]
        public int Eieid { get; set; }
        public int IndicatorId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? EventDateTime { get; set; }
        [StringLength(50)]
        public string EventTime { get; set; }
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

        [ForeignKey(nameof(IndicatorId))]
        [InverseProperty(nameof(EconomicIndicators.EconomicIndicatorEvents))]
        public virtual EconomicIndicators Indicator { get; set; }
    }
}
