using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL_SqlServer_Models.Models
{
    public partial class EconomicIndicators
    {
        public EconomicIndicators()
        {
            EconomicIndicatorEvents = new HashSet<EconomicIndicatorEvents>();
        }

        [Key]
        public int IndicatorId { get; set; }
        public int CountryId { get; set; }
        [Required]
        [StringLength(256)]
        public string IndicatorName { get; set; }
        public bool Active { get; set; }

        [ForeignKey(nameof(CountryId))]
        [InverseProperty(nameof(Countries.EconomicIndicators))]
        public virtual Countries Country { get; set; }
        [InverseProperty("Indicator")]
        public virtual ICollection<EconomicIndicatorEvents> EconomicIndicatorEvents { get; set; }
    }
}
