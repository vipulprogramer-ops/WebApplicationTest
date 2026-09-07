using System;
using System.Web.UI;

namespace UserLoginHistory
{
    public partial class Register : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtUsername.Focus();
            }
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }

            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;
            string salt = PasswordHasher.GenerateSalt();
            string hash = PasswordHasher.HashPassword(password, salt);

            try
            {
                UserRepository repository = new UserRepository();

                if (!repository.CreateUser(username, hash, salt))
                {
                    ShowMessage("That username is already in use.");
                    return;
                }

                Response.Redirect("Login.aspx?registered=1", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception)
            {
                ShowMessage("We could not create your account. Please try again.");
            }
        }

        private void ShowMessage(string message)
        {
            pnlMessage.Visible = true;
            lblMessage.Text = Server.HtmlEncode(message);
        }
    }
}
