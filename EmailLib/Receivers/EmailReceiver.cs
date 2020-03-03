using Newtonsoft.Json;
using RMQLib;
using RMQLib.Messages.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static System.Console;

namespace EmailLib.Receivers
{
    public class EmailReceiver : Receiver
    {
        private readonly RabbitContext _ctx;

        public EmailReceiver(RabbitContext ctx) :
            base(ctx)
        {
            _ctx = ctx;
            SetRoutingKey("email.queue", "cvemail");
        }
        public void Run()
        {
            base.Register();

            WriteLine("started email receiver");
            WriteLine(" Press [enter] to exit.");
            ReadLine();
        }

        public override bool Process(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return true;

            var msg = JsonConvert.DeserializeObject<EmailMessage>(json);

            return true;
        }
    }
}
