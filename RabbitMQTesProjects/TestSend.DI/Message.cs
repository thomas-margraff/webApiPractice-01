using System;
using System.Collections.Generic;
using System.Text;

namespace TestSend.DI
{
    public class Message
    {
        public string Name { get; set; }

        public bool Flag { get; set; }

        public int Index { get; set; }

        public IEnumerable<int> Numbers { get; set; }
    }
}
