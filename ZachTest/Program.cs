using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace ZachTest
{
    class Program
    {
        static void Main(string[] args)
        {
        //    string sqlConn = "Data Source=TOM-HPENVY-16;Initial Catalog=ZachTest;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"

        //    using (sqlConn = new SqlConnection(SqlconString))
        //    {
        //        sqlCon.Open();
        //        SqlCommand sql_cmnd = new SqlCommand("PROC_NAME", sqlCon);
        //        sql_cmnd.CommandType = CommandType.StoredProcedure;
        //        sql_cmnd.Parameters.AddWithValue("@FIRST_NAME", SqlDbType.NVarChar).Value = firstName;
        //        sql_cmnd.Parameters.AddWithValue("@LAST_NAME", SqlDbType.NVarChar).Value = lastName;
        //        sql_cmnd.Parameters.AddWithValue("@AGE", SqlDbType.Int).Value = age;
        //        sql_cmnd.ExecuteNonQuery();
        //        sqlCon.Close();
        //    }
        }
    }
}
