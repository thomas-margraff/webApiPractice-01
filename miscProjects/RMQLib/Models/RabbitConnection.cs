using System;
using System.Collections.Generic;
using System.Text;

namespace RMQLib.Models
{
    public class RabbitConnection
    {
        public string HostName { get; set; } = "gull-01.rmq.cloudamqp.com";
        public int Port { get; set; } = 5672;
        public string UserName { get; set; } = "noekmbda";
        public string Password { get; set; } = "jtKtg3LU2uEQOCJRpEgMhdSqf2N7S_Cv";
        public string VirtualHost { get; set; } = "noekmbda";
        public bool AutomaticRecoveryEnabled { get; set; } = true;
        public bool TopologyRecoveryEnabled { get; set; } = true;
        public int RequestedConnectionTimeout { get; set; } = 60000;
        public ushort RequestedHeartbeat { get; set; } = 60;

        public RabbitConnection()
        {
        }

        public void ClearDefaults()
        {
            HostName = string.Empty;
            UserName = null;
            Password = null;
            VirtualHost = null;
        }
    }
}
