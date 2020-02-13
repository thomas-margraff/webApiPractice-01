using System;
using System.Collections.Generic;
using System.Text;

namespace CoronaVirusDAL.Entities
{
    public class ImportLog
    {
        public int Id { get; set; }
        public string FilePath { get; set; }
        public DateTime DateCreated { get; set; }

    }
}
