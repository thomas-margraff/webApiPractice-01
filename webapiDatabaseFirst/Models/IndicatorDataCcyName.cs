using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webapiDatabaseFirst.Models
{
    public partial class IndicatorDataCcyName
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(3)]
        public string Currency { get; set; }
        [Required]
        [StringLength(120)]
        public string Indicator { get; set; }
        public bool? IsActive { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreateDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ModifyDate { get; set; }
    }
}
