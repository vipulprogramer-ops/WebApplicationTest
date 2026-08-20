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
    public partial class GRDAdd : System.Web.UI.Page
    {
        private string connStr = ConfigurationManager.ConnectionStrings["VSConnectionTestConnectionString"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGrid();
            }
        }
        private void BindGrid()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT ID, TestName FROM SampleData";
                using (SqlDataAdapter sda = new SqlDataAdapter(query, conn))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                }

            }
        }

        private void BindGridSort(string sortExpression, string sortDirection)
        {
            DataTable dt = GetData();

            if (dt != null)
            {
                // Wrap DataTable into a DataView to sort it dynamically
                DataView dv = dt.DefaultView;
                dv.Sort = sortExpression + " " + sortDirection;

                GridView1.DataSource = dv;
                GridView1.DataBind();
            }
        }

        public DataTable GetData()
        {
            // 2. Define your SQL Select statement
            string selectQuery = "SELECT * FROM SampleData";

            // Create an empty DataTable to hold the results
            DataTable dt = new DataTable();

            // 3. Initialize connection and command objects inside using blocks
            using (SqlConnection conn = new SqlConnection(connStr))
            {

                using (SqlCommand cmd = new SqlCommand(selectQuery, conn))
                {
                    // 4. Create the DataAdapter and pass the command object to it
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        // 5. Open the connection and fill the DataTable
                        conn.Open();
                        adapter.Fill(dt); // DataAdapter automatically manages opening/closing if preferred, but manual Open is safe
                    }
                }
            }

            // 6. Return the populated DataTable object
            return dt;
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("AddNew"))
            {
                GridViewRow footerRaw = GridView1.FooterRow;

                TextBox txtName = (TextBox)footerRaw.FindControl("txtNewName");

                if (string.IsNullOrEmpty(txtName.Text))
                {
                    txtName.BorderColor = System.Drawing.Color.Red;
                    return;
                }

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    string insertQuery = @"INSERT INTO SampleData (TestName) VALUES (@TestName)";

                    using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@TestName", txtName.Text.Trim());
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }

                }
                BindGrid();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Success", "alert('Insert Data Added successfully!');", true);

            }
        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex; // Shifts target row into text-edit display state
            string sortExpr = ViewState["SortExpression"] != null ? ViewState["SortExpression"].ToString() : "ID";
            string sortDir = ViewState["SortDirection"] != null ? ViewState["SortDirection"].ToString() : "ASC";


            //BindGrid();
            BindGridSort(sortExpr, sortDir);
        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            BindGrid();
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int id = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Value);
            GridViewRow row = GridView1.Rows[e.RowIndex];

            TextBox txtName = (TextBox)row.FindControl("txtEditName");
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string updateQuery = "UPDATE SampleData SET TestName=@TestName WHERE ID=@ID";
                using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@TestName", txtName.Text.Trim());
                    cmd.Parameters.AddWithValue("@ID", id);
                    conn.Open();
                    cmd.ExecuteNonQuery();

                }
            }
            GridView1.EditIndex = -1;
            BindGrid();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Success", "alert('Row Updated successfully!');", true);

        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int id = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Value);

            using (SqlConnection conn = new SqlConnection(connStr))
            {

                string deleteQuery = "DELETE FROM SampleData WHERE ID=@ID";

                using (SqlCommand cmd = new SqlCommand(deleteQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@ID", id);
                    conn.Open();
                    cmd.ExecuteNonQuery();

                    //Re-bind the sorted data
                    string sortExpr = ViewState["SortExpression"] != null ? ViewState["SortExpression"].ToString() : "ID";
                    string sortDir = ViewState["SortDirection"] != null ? ViewState["SortDirection"].ToString() : "ASC";

                    BindGridSort(sortExpr, sortDir);
                }
            }
            //BindGrid();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Success", "alert('Row Deleted successfully!');", true);
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            // 1. Determine the sort direction toggle
            string sortDirection = "ASC";
            if (ViewState["SortExpression"] != null && ViewState["SortExpression"].ToString() == e.SortExpression)
            {
                // If clicking the same column, flip the direction
                sortDirection = (ViewState["SortDirection"].ToString() == "ASC") ? "DESC" : "ASC";
            }

            // 2. Save current state to ViewState for the next click
            ViewState["SortExpression"] = e.SortExpression;
            ViewState["SortDirection"] = sortDirection;

            // 3. Re-bind the sorted data
            BindGridSort(e.SortExpression, sortDirection);
        }
    }
}

