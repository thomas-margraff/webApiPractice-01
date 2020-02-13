using CoronaVirusDAL.Entities;
using CoronaVirusLib.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base.Enums;
using TableDependency.SqlClient.Base.EventArgs;

namespace cvDBTester
{
    public class SQLNotifications
    {
        private readonly cvConfig cvConfiguration;

        public SQLNotifications(cvConfig cvConfiguration)
        {
            this.cvConfiguration = cvConfiguration;
        }

        // https://github.com/christiandelbianco/monitor-table-change-with-sqltabledependency
        public void Run()
        {
            using (var tableDependency = new SqlTableDependency<ScrapeRun>(this.cvConfiguration.ConnectionString()))
            {
                tableDependency.OnChanged += TableDependency_Changed;
                tableDependency.OnStatusChanged += TableDependency_OnStatusChanged;
                tableDependency.Start();

                Console.WriteLine("Waiting for receiving notifications...");
                Console.WriteLine("Press a key to stop");
                Console.ReadKey();
            }
        }

        void TableDependency_Changed(object sender, RecordChangedEventArgs<ScrapeRun> e)
        {
            if (e.ChangeType != ChangeType.None)
            {
                var changedEntity = e.Entity;
                Console.WriteLine("DML operation: " + e.ChangeType);
                Console.WriteLine("ID: " + changedEntity.Id);
                Console.WriteLine("Description: " + changedEntity.Heading);
            }
        }

        private void TableDependency_OnStatusChanged(object sender, StatusChangedEventArgs e)
        {
            Console.WriteLine(@"Status: " + e.Status);
        }

        private void TableDependency_OnError(object sender, ErrorEventArgs e)
        {
            throw e.Error;
        }


    }
}
