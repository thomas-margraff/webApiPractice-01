using System;
using System.Collections.Generic;
using System.Text;

namespace RMQLib.Models
{
    public class RabbitQueue
    {
        public string Name { get; set; }
        public bool Durable { get; set; }
        public bool Exclusive { get; set; }
        public bool AutoDelete { get; set; }
        public IDictionary<string, object> Arguments { get; set; } = null;

        public RabbitQueue()
        {

        }        
    }
}
