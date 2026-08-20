using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;

namespace UserLoginHistory
{
    public class DBHelper
    {
        public static SqlConnection GetConnection()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyApplicationDB"].ConnectionString;

            return new SqlConnection(connectionString);
        }
    }
}