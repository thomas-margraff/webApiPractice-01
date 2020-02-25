using RMQLib.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestSend
{
    public class Logger
    {
        private AppLogMessage logMsg;
        public Logger() 
        {
            logMsg = new AppLogMessage();
        }

        public void Run()
        {
        }

    }
}
