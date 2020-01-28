using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL_SqlServer_Models.Models
{
    public partial class IndicatorData
    {
        [Key]
        public int Id { get; set; }
        public int EventId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime ReleaseDateTime { get; set; }
        [Required]
        [StringLength(20)]
        public string ReleaseDate { get; set; }
        [Required]
        [StringLength(20)]
        public string ReleaseTime { get; set; }
        [Required]
        [StringLength(3)]
        public string Currency { get; set; }
        [Required]
        [StringLength(120)]
        public string Indicator { get; set; }
        [StringLength(20)]
        public string Actual { get; set; }
        [StringLength(20)]
        public string Forecast { get; set; }
        [StringLength(20)]
        public string Previous { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreateDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ModifyDate { get; set; }
    }
}
