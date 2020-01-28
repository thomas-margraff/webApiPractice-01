using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webapiDatabaseFirst.Models
{
    public partial class Countries
    {
        public Countries()
        {
            EconomicIndicators = new HashSet<EconomicIndicators>();
        }

        [Key]
        public int CountryId { get; set; }
        [Required]
        [StringLength(3)]
        public string CountryCode { get; set; }
        public bool Active { get; set; }
        [Column(TypeName = "ntext")]
        public string Description { get; set; }

        [InverseProperty("Country")]
        public virtual ICollection<EconomicIndicators> EconomicIndicators { get; set; }
    }
}
