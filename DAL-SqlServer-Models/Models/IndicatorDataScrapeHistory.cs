﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL_SqlServer_Models.Models
{
    public partial class IndicatorDataScrapeHistory
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime ScrapeDate { get; set; }
        public int RecordCount { get; set; }
    }
}
