using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL_SqlServer_Models.Models
{
    public partial class Symbols
    {
        [Key]
        public int SymbolId { get; set; }
        [Required]
        [StringLength(6)]
        public string SymbolCode { get; set; }
        public bool IsActive { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DateCreate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DateModify { get; set; }
    }
}
