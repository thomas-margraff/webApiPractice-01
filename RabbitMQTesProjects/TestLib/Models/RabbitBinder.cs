using System;
using System.Collections.Generic;
using System.Text;

namespace TestLib.Models
{
    public class RabbitBinder
    {
        public string ExchangeName { get; set; }
        public string QueueName { get; set; }
        public string RoutingKey { get; set; }

        public RabbitBinder()
        {

        }
    }
}
