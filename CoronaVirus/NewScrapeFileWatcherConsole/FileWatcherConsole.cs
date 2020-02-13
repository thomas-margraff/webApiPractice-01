using CoronaVirusDAL;
using CoronaVirusLib;
using CoronaVirusLib.Configuration;
using CoronaVirusLib.Parsers;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace NewScrapeFileWatcherConsole
{
    public class FileWatcherConsole
    {
        private readonly CvContext ctx;
        private readonly IConfiguration cfg;
        private readonly ImportScrapeData importData;
        private readonly cvParsers cvParsers;
        private readonly cvConfig cvConfiguration;

        public FileWatcherConsole(CvContext ctx,
                                  IConfiguration cfg,
                                  ImportScrapeData importData,
                                  cvParsers cvParsers,
                                  cvConfig cvConfiguration)
        {
            this.ctx = ctx;
            this.cfg = cfg;
            this.importData = importData;
            this.cvParsers = cvParsers;
            this.cvConfiguration = cvConfiguration;
        }
        public void Run()
        {
            watchForNewFile();
        }
        void watchForNewFile()
        {
            try
            {
                FileSystemWatcher watcher = new FileSystemWatcher();
                watcher.Path = cvConfiguration.JsonDir(); ;
                watcher.IncludeSubdirectories = true;

                // Watch for changes specified in the NotifyFilters enumeration.  
                watcher.NotifyFilter = NotifyFilters.Attributes |
                NotifyFilters.CreationTime |
                NotifyFilters.DirectoryName |
                NotifyFilters.FileName;
                watcher.Filter = "*.*";
                watcher.Created += new FileSystemEventHandler(OnChanged);

                //Start monitoring.  
                watcher.EnableRaisingEvents = true;

                // Wait for user to quit program.  
                Console.WriteLine("Press \'q\' to quit...");
                Console.WriteLine();

                //Make an infinite loop till 'q' is pressed.  
                while (Console.Read() != 'q') ;
            }
            catch (IOException e)
            {
                Console.WriteLine("A Exception Occurred :" + e);
            }
            catch (Exception oe)
            {
                Console.WriteLine("An Exception Occurred :" + oe);
            }
        }
        // Define the event handlers.  
        void OnChanged(object source, FileSystemEventArgs e)
        {
            writeScrapeRunToDB(e.FullPath);
        }
        void writeScrapeRunToDB(string file)
        {
            Console.WriteLine("{0} New scrape file: {1}", DateTime.Now, new FileInfo(file).Name);

            // sleep 5 seconds - let the file get written...
            Thread.Sleep(15000);
            importData.ImportScrapeFile(file);
        }

    }
}
