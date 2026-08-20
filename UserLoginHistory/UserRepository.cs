using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace UserLoginHistory
{
    public class UserRepository
    {
        // =====================================================
        // Get user by username
        // =====================================================

        public DataTable GetUserByUsername(string username)
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = DBHelper.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("dbo.sp_GetUserByUsername", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@p_Username", SqlDbType.VarChar, 50).Value = username;

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }

            return dt;
        }

        // =====================================================
        // Record failed login
        // =====================================================

        public void RecordFailedLogin(int userId)
        {
            using (SqlConnection con = DBHelper.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("sp_RecordFailedLogin",con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@UserId",SqlDbType.Int).Value = userId;

                    con.Open();

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // =====================================================
        // Reset failed login
        // =====================================================

        public void ResetFailedLogin(int userId)
        {
            using (SqlConnection con = DBHelper.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("sp_ResetFailedLogin", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@UserId",SqlDbType.Int).Value =  userId;

                    con.Open();

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // =====================================================
        // Create login audit
        // =====================================================

        public void CreateLoginAudit(int? userId, string username, string ipAddress, bool isSuccess, string failureReason)
        {
            using (SqlConnection con = DBHelper.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("sp_CreateLoginAudit", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (userId.HasValue)
                    {
                        cmd.Parameters.Add("@UserId",SqlDbType.Int).Value = userId.Value;
                    }
                    else
                    {
                        cmd.Parameters.Add("@UserId",SqlDbType.Int).Value = DBNull.Value;
                    }


                    cmd.Parameters.Add("@Username",SqlDbType.VarChar, 50).Value = username;

                    cmd.Parameters.Add("@IPAddress",SqlDbType.VarChar, 45).Value = ipAddress;

                    cmd.Parameters.Add("@IsSuccess",SqlDbType.TinyInt).Value = isSuccess;


                    if (failureReason != null)
                    {
                        cmd.Parameters.Add("@FailureReason",SqlDbType.VarChar,255).Value = failureReason;
                    }
                    else
                    {
                        cmd.Parameters.Add("@FailureReason",SqlDbType.VarChar,255).Value = DBNull.Value;
                    }

                    con.Open();

                    cmd.ExecuteNonQuery();
                }
            }
        }


    }
}

    
