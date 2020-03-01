using System;
using System.IO;
using EasyNetQ;
using EasyNetQ.Common.Connection;
using EasyNetQ.Common.Messages;
using EasyNetQ.Common.Messages.Download;
using ForexPriceLib.Utils;
using static System.Console;

namespace ReceiveDownload
{
    class Program
    {
        static string forexiteSaveToPath = @"C:\devApps\webApiPractice-01\EasyNetQ\Forexite\ZipFiles";

        static void Main(string[] args)
        {
            using (var bus = RabbitHutch.CreateBus(Connection.Local()))
            {
                bus.Receive<ForexiteDownloadMessage>("download.forexite",
                    message => HandleDownloadFileMessage(message));

                Console.WriteLine("Listening for forexite download messages. Hit <return> to quit.");
                ReadLine();
            }

        }

        static void HandleDownloadFileMessage(ForexiteDownloadMessage msg)
        {
            int i = 0;

            var localFileName = FileUtils.DateToFxiFileName(msg.DownloadDataForDate, ".zip");
            var saveFpath = Path.Combine(forexiteSaveToPath, localFileName);
            File.WriteAllBytes(saveFpath, msg.FileBytes);
        }

    }
}
