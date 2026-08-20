using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

namespace UserLoginHistory
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtUsername.Focus();

                pnlMessage.Visible = false;
            }
        }

        private void ShowMessage(string message)
        {
                pnlMessage.Visible = true;

                lblMessage.Text =  Server.HtmlEncode(message);
        }

        protected void btnLogin_Click1(object sender, EventArgs e)
        {
            try
            {
                string username = txtUsername.Text.Trim();

                string password = txtPassword.Text;

                string ipAddress = GetClientIPAddress();

                AuthenticationService1 auth = new AuthenticationService1();

                AuthenticationResult1 result = auth.Login(username,password,ipAddress);


                // -------------------------------------------------
                // Login failed
                // -------------------------------------------------

                if (!result.Success)
                {
                    pnlMessage.Visible = true;
                    lblMessage.Text = Server.HtmlEncode(result.Message);
                    return;
                }


                // -------------------------------------------------
                // Login successful
                // -------------------------------------------------

                Session["UserId"] = result.UserId;

                Session["Username"]  = result.Username;

                Session["Authenticated"] = true;

                Response.Redirect("Default.aspx",false);

                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                // Development only
                lblMessage.Text = Server.HtmlEncode(ex.ToString());
            }
        }

        private string GetClientIPAddress()
        {
            string ip = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!String.IsNullOrEmpty(ip))
            {
                string[] addresses = ip.Split(',');

                if (addresses.Length > 0)
                {
                    return addresses[0].Trim();
                }
            }


            ip = Request.ServerVariables["REMOTE_ADDR"];

            if (String.IsNullOrEmpty(ip))
            {
                ip = "UNKNOWN";
            }
            return ip;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                AuditRepository repo = new AuditRepository();

                repo.ResetFailedLogin(1);

                Response.Write("ResetFailedLogin SUCCESS");
            }
            catch (Exception ex)
            {
                Response.Write("<pre>" + Server.HtmlEncode(ex.ToString()) + "</pre>");
            }
        }
    }
}
