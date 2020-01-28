using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webapiDatabaseFirst.Models
{
    public partial class VwCountryIndicator
    {
        [Required]
        [StringLength(3)]
        public string Currency { get; set; }
        [Required]
        [StringLength(120)]
        public string Indicator { get; set; }
    }
}
