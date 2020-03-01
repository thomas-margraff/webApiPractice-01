using System;
using System.Collections.Generic;
using System.Text;

namespace EasyNetQ.Common.Messages
{
    public interface IBaseMessage
    {
        public DateTime TimeStamp { get; set; }
    }

    public class BaseMessage
    {
        public DateTime TimeStamp { get; set; } = DateTime.Now;
    }

}
