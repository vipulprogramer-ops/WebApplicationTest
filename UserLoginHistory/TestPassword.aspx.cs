using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
namespace UserLoginHistory
{
    public partial class TestPassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string username = "admin";
                string password = "admin123";

                string cs =
                    "Data Source=HP;" +
                    "Initial Catalog=MyApplicationDB;" +
                    "Integrated Security=True;";

                DataTable dt =
                    new DataTable();

                using (SqlConnection con =
                       new SqlConnection(cs))
                {
                    using (SqlCommand cmd =
                           new SqlCommand(
                               "dbo.sp_GetUserByUsername",
                               con))
                    {
                        cmd.CommandType =
                            CommandType.StoredProcedure;

                        cmd.Parameters.Add(
                            "@p_Username",
                            SqlDbType.VarChar,
                            50).Value =
                            username;

                        using (SqlDataAdapter da =
                               new SqlDataAdapter(cmd))
                        {
                            da.Fill(dt);
                        }
                    }
                }

                if (dt.Rows.Count == 0)
                {
                    Response.Write(
                        "USER NOT FOUND");

                    return;
                }

                string storedHash =
                    Convert.ToString(
                        dt.Rows[0]["PasswordHash"]);

                string storedSalt =
                    Convert.ToString(
                        dt.Rows[0]["PasswordSalt"]);

                Response.Write(
                    "User found<br/><br/>");

                Response.Write(
                    "Username: " +
                    Server.HtmlEncode(username));

                Response.Write("<br/>");

                Response.Write(
                    "Hash length: " +
                    storedHash.Length);

                Response.Write("<br/>");

                Response.Write(
                    "Salt length: " +
                    storedSalt.Length);

                Response.Write("<br/><br/>");

                bool valid =
                    PasswordHasher.VerifyPassword(
                        password,
                        storedHash,
                        storedSalt);

                Response.Write(
                    "Password verification: " +
                    valid);
            }
            catch (Exception ex)
            {
                Response.Write(
                    "<pre>" +
                    Server.HtmlEncode(
                        ex.ToString()) +
                    "</pre>");
            }


        }
    }
}