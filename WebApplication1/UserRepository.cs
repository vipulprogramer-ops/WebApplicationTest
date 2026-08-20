using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace WebApplication1
{
    public class UserRepository
    {
        public DataTable Login(string username, string passwordHash)
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = DBHelper.GetConnectionNew())
            {
                using (SqlCommand cmd = new SqlCommand("sp_UserLogin", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@p_Username", username);
                    cmd.Parameters.AddWithValue("@p_PasswordHash", passwordHash);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }

            return dt;
        }
    }
}
