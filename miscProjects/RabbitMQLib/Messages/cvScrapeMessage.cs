using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQLib.Messages
{
    public class cvScrapeMessage: BaseMessage 
    {
        public cvScrapeMessage() :base()
        {

        }

        public cvScrapeMessage(string queueName, string routingKeyName, string message)
        {
        }
    }
}
