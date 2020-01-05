using System;
using System.Collections.Generic;
using System.Text;

namespace DAL_SqlServer.Models
{
    public class IndicatorDataCcyName
    {
        public int Id { get; set; }
        public string Currency { get; set; }
        public string Indicator { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? ModifyDate { get; set; }
    }
}
