using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class TestData : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            string hash = PasswordHelper.SHA256Hash("admin123");

            Response.Write(hash);
        }
    }
}