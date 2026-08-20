using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace UserLoginHistory
{
    public partial class GeneratePassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            
            string password = "admin123";

            string salt = PasswordHasher.GenerateSalt();

            string hash = PasswordHasher.HashPassword(
                    password,
                    salt);

            lblResult.Text =
                "Password: " + password +
                "<br/><br/>" +
                "Salt: " + salt +
                "<br/><br/>" +
                "Hash: " + hash;
        }
    }
}