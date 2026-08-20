using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace WebApplication1
{
    public class DBHelper
    {
        public static SqlConnection GetConnectionNew()
        {
            string connectionString1 = ConfigurationManager.ConnectionStrings["VSConnectionTestConnectionString"].ConnectionString;

            return new SqlConnection(connectionString1);
        }
    }
}