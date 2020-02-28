using System;
using System.Collections.Generic;
using System.Text;

namespace EasyNetQ.Common.Connection
{
    public class Connection
    {
        public static string CloudAmqp()
        {
            return "host=gull-01.rmq.cloudamqp.com;virtualHost=noekmbda;username=noekmbda;password=jtKtg3LU2uEQOCJRpEgMhdSqf2N7S_Cv";
        }
        public static string Local()
        {
            return "host=localhost";
        }
    }
}
