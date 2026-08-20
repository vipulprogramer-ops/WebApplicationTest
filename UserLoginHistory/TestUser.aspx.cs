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
    public partial class TestUser : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string username = "admin";

                string cs =
                    "Data Source=HP;" +
                    "Initial Catalog=MyApplicationDB;" +
                    "Integrated Security=True;";

                DataTable dt = new DataTable();

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

                Response.Write(
                    "Rows returned: " +
                    dt.Rows.Count);

                if (dt.Rows.Count > 0)
                {
                    Response.Write("<br/><br/>");

                    Response.Write(
                        "UserId: " +
                        dt.Rows[0]["UserId"]);

                    Response.Write("<br/>");

                    Response.Write(
                        "Username: " +
                        dt.Rows[0]["Username"]);

                    Response.Write("<br/>");

                    Response.Write(
                        "IsActive: " +
                        dt.Rows[0]["IsActive"]);

                    Response.Write("<br/>");

                    Response.Write(
                        "Failed Attempts: " +
                        dt.Rows[0]["FailedLoginAttempts"]);

                    Response.Write("<br/>");

                    Response.Write(
                        "LockoutUntil: " +
                        dt.Rows[0]["LockoutUntil"]);
                }
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