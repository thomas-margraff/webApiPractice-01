using System;

namespace ConsoleLogger
{
    class Program
    {
        static void Main(string[] args)
        {
            RabbitMQReceiver receiver = new RabbitMQReceiver();
            receiver.Receive();

            //Console.WriteLine(" Press [enter] to exit.");
            //Console.ReadLine();

        }
    }
}
