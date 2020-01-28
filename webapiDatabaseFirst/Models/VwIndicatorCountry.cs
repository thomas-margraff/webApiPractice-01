using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webapiDatabaseFirst.Models
{
    public partial class VwIndicatorCountry
    {
        public int CountryId { get; set; }
        [Required]
        [StringLength(3)]
        public string CountryCode { get; set; }
        public int IndicatorId { get; set; }
        [Required]
        [StringLength(256)]
        public string IndicatorName { get; set; }
        public bool IndicatorActive { get; set; }
        public bool CountryActive { get; set; }
    }
}
