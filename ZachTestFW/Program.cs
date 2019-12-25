using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace ZachTestFW
{
    public class Person
    {
        public int PersonId { get; set; }
        public string NameFirst { get; set; }
        public string NameLast { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var dt = new DateTime(2013, 1, 1);
            List<Person> peeps = GetPersonsWithOrders(dt);
        }

        static List<Person> GetPersonsWithOrders(DateTime dt)
        {
            List<Person> peeps = new List<Person>();
            var conn = ConfigurationManager.ConnectionStrings["zachTestDb"].ConnectionString;
            SqlConnection sqlCon = null;

            using (sqlCon = new SqlConnection(conn))
            {
                sqlCon.Open();
                SqlCommand sql_cmnd = new SqlCommand("spGetPersonsWithOrders", sqlCon);
                sql_cmnd.CommandType = CommandType.StoredProcedure;
                sql_cmnd.Parameters.AddWithValue("@orderDate", SqlDbType.Date).Value = dt;

                SqlDataReader reader = sql_cmnd.ExecuteReader();

                while (reader.Read())
                {
                    peeps.Add(new Person
                    {
                        PersonId = Convert.ToInt32(reader[0].ToString()),
                        NameFirst = reader[1].ToString(),
                        NameLast = reader[2].ToString(),
                    });
                }
                sqlCon.Close();
            }

            return peeps;
        }
    }
}
