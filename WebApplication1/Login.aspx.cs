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
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtUsername.Focus();
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }

            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            // Convert password to SHA-256
            string passwordHash = PasswordHelper.SHA256Hash(password);

            try
            {
                UserRepository repository = new UserRepository();

                DataTable dt = repository.Login(username,passwordHash);

                if (dt.Rows.Count > 0)
                {
                    // Login successful

                    Session["UserId"] = dt.Rows[0]["UserId"].ToString();
                    Session["Username"] = dt.Rows[0]["Username"].ToString();

                    Response.Redirect("Dashboard.aspx",false);

                    Context.ApplicationInstance.CompleteRequest();
                }
                else
                {
                    lblMessage.Text = "Invalid username or password.";
                }
            }
            catch (Exception)
            {
                lblMessage.Text = "Unable to process login.";
            }
        }
    }
}