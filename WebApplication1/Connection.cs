using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace WebApplication1
{
    public class Connection
    {
        // Store your connection string in one single place
        private static readonly string connString = ConfigurationManager.ConnectionStrings["VSConnectionTestConnectionString"].ConnectionString;
        // "Data Source=HP;Initial Catalog=VSConnectionTest;User ID=sa;Password=Admin@123";

        // The common method all your events and functions will call
        public static SqlConnection GetConnection()
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                return conn; // Returns an opened connection ready for use
            }
            
        }
    }
}